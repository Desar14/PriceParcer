using MediatR;
using PriceParser.Core.DTO;

namespace PriceParser.CQS.Models.Queries
{
    public class GetProductFromSitePricesQuery : IRequest<IEnumerable<ProductPriceDTO>>
    {
        public Guid ProductId { get; set; }
        public Guid ProductFromSiteId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool PerEveryDay { get; set; }
    }
}
