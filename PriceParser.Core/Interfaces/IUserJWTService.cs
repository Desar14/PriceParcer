using PriceParser.Core.DTO;

namespace PriceParser.Core.Interfaces
{
    public interface IUserJWTService
    {
        Task<AuthenticateResponseDTO> AuthenticateAsync(string username, string password, string ipAddress);
        Task<AuthenticateResponseDTO> RefreshTokenAsync(string token, string ipAddress);
        bool RevokeToken(string token, string ipAddress);
    }
}
