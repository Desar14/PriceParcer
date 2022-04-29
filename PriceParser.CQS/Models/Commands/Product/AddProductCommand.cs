using MediatR;
using PriceParser.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
