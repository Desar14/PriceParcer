using AutoMapper;
using PriceParcer.Core.DTO;
using PriceParcer.Core.Interfaces;

namespace PriceParcer.Domain
{
    public class ProductService : IProductsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            return (await _unitOfWork.Products.Get())
                .Select(product => _mapper.Map<ProductDTO>(product));
        }
    }
}