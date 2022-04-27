using PriceParser.Data.Entities;

namespace PriceParser.Core.DTO
{
    public class AuthenticateResponseDTO
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string JwtToken { get; set; }
        public DateTime JwtValidTo { get; set; }

        //[JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }
        public DateTime RefreshValidTo { get; set; }

        public AuthenticateResponseDTO(ApplicationUser user, string jwtToken, string refreshToken, DateTime _jwtValidTo, DateTime _refreshValidTo)
        {
            Id = user.Id;
            Username = user.UserName;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
            JwtValidTo = _jwtValidTo;
            RefreshValidTo = _refreshValidTo;
        }
    }
}
