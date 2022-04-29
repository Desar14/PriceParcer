using MediatR;
using PriceParser.Core.DTO;

namespace PriceParser.CQS.Models.Commands
{

    public class UpdateProductPriceCommand : IRequest<bool>
    {
        public UpdateProductPriceCommand(ProductPriceDTO productPrice)
        {
            ProductPrice = productPrice;
        }

        public ProductPriceDTO ProductPrice { get; set; }
    }
}
