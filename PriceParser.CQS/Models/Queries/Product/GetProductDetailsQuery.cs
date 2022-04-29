using MediatR;
using PriceParser.Core.DTO;

namespace PriceParser.CQS.Models.Queries
{
    public class GetProductDetailsQuery : IRequest<ProductDTO>
    {
        public GetProductDetailsQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
