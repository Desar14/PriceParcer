using MediatR;
using PriceParser.Core.DTO;

namespace PriceParser.CQS.Models.Queries
{
    public class GetAllProductPricesQuery : IRequest<IEnumerable<ProductPriceDTO>>
    {
        public GetAllProductPricesQuery(Guid _productFromSitesId)
        {
            ProductFromSitesId = _productFromSitesId;
        }

        public Guid ProductFromSitesId { get; set; }
    }
}
