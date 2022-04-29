using MediatR;
using PriceParser.Core.DTO;

namespace PriceParser.CQS.Models.Queries
{
    public class GetAllProductsQuery : IRequest<IEnumerable<ProductDTO>>
    {
    }
}
