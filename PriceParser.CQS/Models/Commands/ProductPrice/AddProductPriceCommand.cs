using MediatR;
using PriceParser.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
