using MediatR;
using PriceParser.Core.DTO;

namespace PriceParser.CQS.Models.Commands
{
    public class AddProductCommand : IRequest<bool>
    {
        public AddProductCommand(ProductDTO product)
        {
            Product = product;
        }

        public ProductDTO Product { get; set; }
    }
}
