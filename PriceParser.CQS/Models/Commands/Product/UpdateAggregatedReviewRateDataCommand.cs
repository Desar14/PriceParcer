using MediatR;

namespace PriceParser.CQS.Models.Commands
{
    public class UpdateAggregatedReviewRateDataCommand : IRequest<bool>
    {
        public UpdateAggregatedReviewRateDataCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
