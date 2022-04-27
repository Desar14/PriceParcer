using PriceParser.Core.DTO;
using PriceParser.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceParser.Core.Interfaces
{
    public interface IUserJWTService
    {
        Task<AuthenticateResponseDTO> AuthenticateAsync(string username, string password, string ipAddress);
        Task<AuthenticateResponseDTO> RefreshTokenAsync(string token, string ipAddress);
        bool RevokeToken(string token, string ipAddress);
    }
}
