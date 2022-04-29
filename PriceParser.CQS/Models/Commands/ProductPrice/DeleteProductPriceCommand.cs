using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceParser.CQS.Models.Commands
{
    public class DeleteProductPriceCommand : IRequest<bool>
    {
        public DeleteProductPriceCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
