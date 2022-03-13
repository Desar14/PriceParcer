using PriceParser.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceParser.Core.Interfaces
{
    public interface IUserReviewsService
    {
        Task<IEnumerable<UserReviewDTO>> GetAllAsync();

        Task<IEnumerable<UserReviewDTO>> GetAllByProductAsync(Guid productId);

        Task<UserReviewDTO> GetDetailsAsync(Guid id);

        Task<bool> AddAsync(UserReviewDTO review);

        Task<bool> EditAsync(UserReviewDTO review);

        Task<bool> DeleteAsync(Guid id);
    }
}
