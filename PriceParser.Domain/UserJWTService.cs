using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PriceParser.Core.DTO;
using PriceParser.Core.Interfaces;
using PriceParser.Data.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PriceParser.Domain
{
    public class UserJWTService : IUserJWTService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public UserJWTService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<AuthenticateResponseDTO> AuthenticateAsync(string username, string password, string ipAddress)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
                return null;

            // authentication successful so generate jwt and refresh tokens
            var jwtToken = await GenerateJwtTokenAsync(user);
            var refreshToken = GenerateRefreshToken(ipAddress);

            // save refresh token
            if (user.RefreshTokens == null)
                user.RefreshTokens = new List<RefreshToken>();

            user.RefreshTokens.Add(refreshToken);
            _context.Update(user);
            _context.SaveChanges();

            var jwtTokenString = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return new AuthenticateResponseDTO(user, jwtTokenString, refreshToken.Token, jwtToken.ValidTo, refreshToken.Expires);
        }

        public async Task<AuthenticateResponseDTO> RefreshTokenAsync(string token, string ipAddress)
        {
            var user = _context.Users.Include(u => u.RefreshTokens.Where(t => t.Token == token)).Where(u => u.RefreshTokens.Any(t => t.Token == token)).SingleOrDefault();

            // return null if no user found with token
            if (user == null) return null;

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            // return null if token is no longer active
            if (!refreshToken.IsActive) return null;

            // replace old refresh token with a new one and save
            var newRefreshToken = GenerateRefreshToken(ipAddress);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;

            if (user.RefreshTokens == null)
                user.RefreshTokens = new List<RefreshToken>();

            user.RefreshTokens.Add(newRefreshToken);
            _context.Update(user);
            _context.SaveChanges();

            // generate new jwt
            var jwtToken = await GenerateJwtTokenAsync(user);

            var jwtTokenString = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return new AuthenticateResponseDTO(user, jwtTokenString, newRefreshToken.Token, jwtToken.ValidTo, newRefreshToken.Expires);
        }

        public bool RevokeToken(string token, string ipAddress)
        {
            var user = _context.Users.Include(u => u.RefreshTokens).SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            // return false if no user found with token
            if (user == null) return false;

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            // return false if token is not active
            if (!refreshToken.IsActive) return false;

            // revoke token and save
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            _context.Update(user);
            _context.SaveChanges();

            return true;
        }

        private async Task<JwtSecurityToken> GenerateJwtTokenAsync(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;//new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static RefreshToken GenerateRefreshToken(string ipAddress)
        {            
            var randomBytes = new byte[64];  
            using RNGCryptoServiceProvider rng = new();
            rng.GetBytes(randomBytes);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomBytes),
                Expires = DateTime.UtcNow.AddDays(1),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }
    }
}
