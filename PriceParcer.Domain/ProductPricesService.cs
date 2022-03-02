using AutoMapper;
using HtmlAgilityPack;
using PriceParcer.Core.DTO;
using PriceParcer.Core.Interfaces;
using PriceParcer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceParcer.Domain
{
    public class ProductPricesService : IProductPricesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IProductsFromSitesService _productsFromSitesService;

        public ProductPricesService(IUnitOfWork unitOfWork, IMapper mapper, IProductsFromSitesService productsFromSitesService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _productsFromSitesService = productsFromSitesService;
        }

        public async Task<bool> AddProductPriceAsync(ProductPriceDTO productPriceDTO)
        {
            var entity = _mapper.Map<ProductPrice>(productPriceDTO);

            await _unitOfWork.ProductPricesHistory.Add(entity);

            var result = await _unitOfWork.Commit();

            return result > 0;
        }

        public async Task<bool> DeleteProductPriceAsync(Guid id)
        {
            await _unitOfWork.ProductPricesHistory.Delete(id);

            var result = await _unitOfWork.Commit();

            return result > 0;
        }

        public async Task<IEnumerable<ProductPriceDTO>> GetAllProductPricesAsync(Guid productFromSitesId)
        {
            return (await _unitOfWork.ProductPricesHistory.Get(filter: price => price.ProductFromSiteId == productFromSitesId, includes: price => price.ProductFromSite))
                .Select(price => _mapper.Map<ProductPriceDTO>(price));
        }

        public async Task<ProductPriceDTO> GetProductPriceDetailsAsync(Guid priceId)
        {
            var result = (await _unitOfWork.ProductPricesHistory.GetByID(priceId, price => price.ProductFromSite));

            return _mapper.Map<ProductPriceDTO>(result);
        }

        public async Task<ProductPriceDTO> ParseProductPriceAsync(Guid productFromSitesId)
        {

            if (productFromSitesId == Guid.Empty)
                throw new ArgumentNullException(nameof(productFromSitesId));

            ProductPriceDTO result = null;

            var productFromSite = await _productsFromSitesService.GetDetailsAsync(productFromSitesId);

            if (productFromSite.Site.ParseType == ParseTypes.Xpath)
            {
                result = new();
                
                var html = productFromSite.Path;

                HtmlWeb web = new HtmlWeb();

                var htmlDoc = web.Load(html);

                var priceRawString = htmlDoc.DocumentNode
                    //.SelectSingleNode("//span[@data-price]").InnerText;
                    .SelectSingleNode(productFromSite.Site.ParsePricePath).InnerText;
                // .Attributes["value"].Value;

                if (priceRawString == null)
                {
                    throw new ArgumentException($"Can't find price in path {productFromSite.Site.ParsePricePath}");
                }

                if (!Double.TryParse(priceRawString, out double priceParsed))
                {
                    throw new ArgumentException($"Can't parse price {priceRawString}");
                }

                string? CurrencyRawString = null;

                try
                {
                    CurrencyRawString = htmlDoc.DocumentNode
                    //.SelectSingleNode("//span[@data-price]").InnerText;
                    .SelectSingleNode(productFromSite.Site.ParseCurrencyPath).InnerText;
                    // .Attributes["value"].Value;
                }
                catch (Exception)
                {

                   
                }
                result.FullPrice = priceParsed;
                result.ParseDate = DateTime.Now;
                result.Id = Guid.NewGuid();
                result.CurrencyCode = CurrencyRawString == null ? "BYN" : CurrencyRawString;
                result.ProductFromSiteId = productFromSitesId;
            }               
           

            return result;

        }

        public async Task<bool> UpdateProductPriceAsync(ProductPriceDTO productPriceDTO)
        {
            var entity = _mapper.Map<ProductPrice>(productPriceDTO);

            await _unitOfWork.ProductPricesHistory.Update(entity);

            var result = await _unitOfWork.Commit();

            return result > 0;
        }
    }
}
