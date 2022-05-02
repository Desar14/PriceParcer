using MediatR;
using PriceParser.Core.DTO;

namespace PriceParser.CQS.Models.Commands
{
    public class AddProductPriceRangeCommand : IRequest<bool>
    {
        public IEnumerable<ProductPriceDTO> productPriceDTOs { get; set; }

        public AddProductPriceRangeCommand(IEnumerable<ProductPriceDTO> _productPriceDTOs)
        {
            productPriceDTOs = _productPriceDTOs;
        }
    }
}
