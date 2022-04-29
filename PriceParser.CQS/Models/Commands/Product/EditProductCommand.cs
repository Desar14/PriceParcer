using MediatR;
using PriceParser.Core.DTO;

namespace PriceParser.CQS.Models.Commands
{

    public class EditProductCommand : IRequest<bool>
    {
        public EditProductCommand(ProductDTO product)
        {
            Product = product;
        }

        public ProductDTO Product { get; set; }
    }
}
