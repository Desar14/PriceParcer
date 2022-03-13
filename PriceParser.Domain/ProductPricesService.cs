using AutoMapper;
using HtmlAgilityPack;
using PriceParser.Core.DTO;
using PriceParser.Core.Interfaces;
using PriceParser.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceParser.Domain
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

                if (htmlDoc == null)
                {
                    throw new ArgumentException($"Can't load link {productFromSite.Path}");
                }
                double priceParsed = GetParsedValueFromHtmlDocument<double>(htmlDoc, productFromSite.Site.ParsePricePath, productFromSite.Site.ParsePriceAttributeName, productFromSite.Path);

                string CurrencyRawString;

                try
                {
                    CurrencyRawString = GetParsedValueFromHtmlDocument<string>(htmlDoc, productFromSite.Site.ParseCurrencyPath, productFromSite.Site.ParseCurrencyAttributeName, productFromSite.Path);
                }
                catch (Exception)
                {
                    //todo log
                    CurrencyRawString = "BYN";
                }

                result.FullPrice = priceParsed;
                result.ParseDate = DateTime.Now;
                result.Id = Guid.NewGuid();
                result.CurrencyCode = CurrencyRawString;
                result.ProductFromSiteId = productFromSitesId;
            }


            return result;

        }

        private T GetParsedValueFromHtmlDocument<T>(HtmlDocument htmlDoc, string? nodePath, string? attributePath, string link)
        {
            
            var node = htmlDoc.DocumentNode.SelectSingleNode(nodePath);

            string? rawString = null;

            if (node != null)
            {
                if (!String.IsNullOrEmpty(attributePath))
                {
                    var attribute = node.Attributes.First(p => p.Name == attributePath);

                    if (attribute == null)
                    {
                        throw new ArgumentException($"Can't find value in path {nodePath} and attribute {attributePath} on link {link}: attribute not found");
                    }
                    else
                        rawString = attribute.Value;
                }
                else
                    rawString = node.InnerText;
            }
            else
            {
                throw new ArgumentException($"Can't find value in path {nodePath} on link {link}: path not found");
            }

            if (String.IsNullOrEmpty(rawString))
            {
                throw new ArgumentException($"Can't find value in path {nodePath}");
            }

            if (typeof(T) == typeof(string))
            {
                return (T)Convert.ChangeType(rawString, typeof(T));
            }
            else if (typeof(T) == typeof(double))
            {
                rawString = rawString.Replace(',', '.');
                rawString = rawString.Replace(" ","");
                if (!Double.TryParse(rawString, NumberStyles.Any, CultureInfo.InvariantCulture, out double doubleValueParsed))
                {
                    throw new ArgumentException($"Can't parse price {rawString}");
                }
                return (T)Convert.ChangeType(doubleValueParsed, typeof(T));

            }
            else
                return default;
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
