using MediatR;

namespace PriceParser.CQS.Models.Commands
{
    public class UpdateAggregatedPricesDataCommand : IRequest<bool>
    {
        public UpdateAggregatedPricesDataCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
