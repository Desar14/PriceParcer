using MediatR;
using PriceParser.Core.DTO;

namespace PriceParser.CQS.Models.Commands
{
    public class AddProductPriceCommand : IRequest<bool>
    {
        public AddProductPriceCommand(ProductPriceDTO productPrice)
        {
            ProductPrice = productPrice;
        }

        public ProductPriceDTO ProductPrice { get; set; }
    }
}
