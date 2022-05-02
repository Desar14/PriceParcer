using MediatR;

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
