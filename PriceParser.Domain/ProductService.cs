using AutoMapper;
using Microsoft.Extensions.Logging;
using PriceParser.Core.DTO;
using PriceParser.Core.Interfaces;
using PriceParser.Data;

namespace PriceParser.Domain
{
    public class ProductService : IProductsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ProductService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            return (await _unitOfWork.Products.Get())
                .Select(product => _mapper.Map<ProductDTO>(product));
        }

        public async Task<ProductDTO> GetProductDetailsAsync(Guid id)
        {
            
            var result = (await _unitOfWork.Products.GetByID(id));

            if (result != null)
            {
                result.FromSites = new(await _unitOfWork.ProductsFromSites.Get(prod => prod.ProductId == id, null, prod => prod.Site));
                result.Reviews = new(await _unitOfWork.UserReviews.Get(prod => prod.ProductId == id, null, prod => prod.User));
            }                       

            return _mapper.Map<ProductDTO>(result);
        }

        async Task<bool> IProductsService.AddProduct(ProductDTO product)
        {
            var entity = _mapper.Map<Product>(product);

            await _unitOfWork.Products.Add(entity);

            var result = await _unitOfWork.Commit();

            return result > 0;
        }

        async Task<bool> IProductsService.DeleteProduct(Guid id)
        {
            await _unitOfWork.Products.Delete(id);

            var result = await _unitOfWork.Commit();
            
            return result > 0;
        }

        async Task<bool> IProductsService.EditProduct(ProductDTO product)
        {
            var entity = _mapper.Map<Product>(product);

            await _unitOfWork.Products.Update(entity);

            var result = await _unitOfWork.Commit();

            return result > 0;
        }
    }
}