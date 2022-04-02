using AutoMapper;
using Microsoft.Extensions.Logging;
using PriceParser.Core.DTO;
using PriceParser.Core.Interfaces;
using PriceParser.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceParser.Domain
{
    public class UserReviewsService : IUserReviewsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UserReviewsService> _logger;

        public UserReviewsService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UserReviewsService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> AddAsync(UserReviewDTO review)
        {
            var entity = _mapper.Map<UserReview>(review);

            await _unitOfWork.UserReviews.Add(entity);

            var result = await _unitOfWork.Commit();

            return result > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await _unitOfWork.UserReviews.Delete(id);

            var result = await _unitOfWork.Commit();

            return result > 0;
        }

        public async Task<bool> EditAsync(UserReviewDTO review)
        {
            var entity = _mapper.Map<UserReview>(review);

            await _unitOfWork.UserReviews.Update(entity);

            var result = await _unitOfWork.Commit();

            return result > 0;
        }

        public async Task<IEnumerable<UserReviewDTO>> GetAllAsync()
        {
            return (await _unitOfWork.UserReviews.Get(null, null, record => record.Product, record => record.User))
                .Select(review => _mapper.Map<UserReviewDTO>(review));
        }

        public async Task<IEnumerable<UserReviewDTO>> GetAllByProductAsync(Guid productId)
        {
            return (await _unitOfWork.UserReviews.Get(record => record.ProductId == productId, null, record => record.User))
                .Select(review => _mapper.Map<UserReviewDTO>(review));
        }

        public async Task<UserReviewDTO> GetDetailsAsync(Guid id)
        {
            var result = (await _unitOfWork.UserReviews.GetByID(id, record => record.Product, record => record.User));

            return _mapper.Map<UserReviewDTO>(result);
        }
    }
}
