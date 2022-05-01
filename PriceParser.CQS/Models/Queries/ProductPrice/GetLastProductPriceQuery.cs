using MediatR;
using PriceParser.Core.DTO;

namespace PriceParser.CQS.Models.Queries
{
    public class GetLastProductPriceQuery : IRequest<ProductPriceDTO>
    {
        public GetLastProductPriceQuery(Guid productFromSiteId)
        {
            ProductFromSiteId = productFromSiteId;
        }

        public Guid ProductFromSiteId { get; set; }
    }
}
