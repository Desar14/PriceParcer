using MediatR;
using PriceParser.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
