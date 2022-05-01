using MediatR;
using PriceParser.Core.DTO;

namespace PriceParser.CQS.Models.Queries
{
    public class GetProductPriceDetailsQuery : IRequest<ProductPriceDTO>
    {
        public GetProductPriceDetailsQuery(Guid priceId)
        {
            PriceId = priceId;
        }

        public Guid PriceId { get; set; }
    }
}
