using MediatR;

namespace PriceParser.CQS.Models.Commands
{
    public class DeleteProductCommand : IRequest<bool>
    {
        public DeleteProductCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
