using PriceParser.Core.DTO;

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
