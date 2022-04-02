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
    public class ProductFromSitesService : IProductsFromSitesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductFromSitesService> _logger;

        public ProductFromSitesService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ProductFromSitesService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> AddAsync(ProductFromSitesDTO product)
        {
            var entity = _mapper.Map<ProductFromSites>(product);

            await _unitOfWork.ProductsFromSites.Add(entity);

            var result = await _unitOfWork.Commit();

            return result > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await _unitOfWork.ProductsFromSites.Delete(id);

            var result = await _unitOfWork.Commit();

            return result > 0;
        }

        public async Task<bool> EditAsync(ProductFromSitesDTO product)
        {
            var entity = _mapper.Map<ProductFromSites>(product);

            await _unitOfWork.ProductsFromSites.Update(entity);

            var result = await _unitOfWork.Commit();

            return result > 0;
        }

        public async Task<IEnumerable<ProductFromSitesDTO>> GetAllAsync()
        {
            return (await _unitOfWork.ProductsFromSites.Get(null,null,record => record.product, record => record.Site, record => record.CreatedByUser))
                .Select(product => _mapper.Map<ProductFromSitesDTO>(product));
        }

        public async Task<IEnumerable<ProductFromSitesDTO>> GetAllByProductAsync(Guid productId)
        {
            return (await _unitOfWork.ProductsFromSites.Get(record => record.ProductId == productId, null,record => record.Site, record => record.CreatedByUser))
                .Select(product => _mapper.Map<ProductFromSitesDTO>(product));
        }

        public async Task<IEnumerable<ProductFromSitesDTO>> GetAllBySiteAsync(Guid siteId)
        {
            return (await _unitOfWork.ProductsFromSites.Get(record => record.SiteId == siteId,null, record => record.product, record => record.CreatedByUser))
                .Select(product => _mapper.Map<ProductFromSitesDTO>(product));
        }

        public async Task<IEnumerable<ProductFromSitesDTO>> GetBySiteForParsingAsync(Guid siteId)
        {
            return (await _unitOfWork.ProductsFromSites.Get(record => record.SiteId == siteId && !record.DoNotParse, null, record => record.product, record => record.CreatedByUser))
                .Select(product => _mapper.Map<ProductFromSitesDTO>(product));
        }

        public async Task<ProductFromSitesDTO> GetDetailsAsync(Guid id)
        {
            var result = (await _unitOfWork.ProductsFromSites.GetByID(id, record => record.product, record => record.Site, record => record.CreatedByUser));           

            return _mapper.Map<ProductFromSitesDTO>(result);
        }
    }
}
