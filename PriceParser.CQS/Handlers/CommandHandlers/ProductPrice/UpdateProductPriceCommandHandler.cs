using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PriceParser.CQS.Models.Commands;
using PriceParser.Data.Entities;

namespace PriceParser.CQS.Handlers.CommandHandlers
{
    public class UpdateProductPriceCommandHandler : IRequestHandler<UpdateProductPriceCommand, bool>
    {
        protected readonly ApplicationDbContext _database;
        protected readonly IMapper _mapper;
        protected readonly ILogger<EditProductCommandHandler> _logger;

        public UpdateProductPriceCommandHandler(ApplicationDbContext database, IMapper mapper, ILogger<EditProductCommandHandler> logger)
        {
            _database = database;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateProductPriceCommand command, CancellationToken token)
        {
            var entity = _mapper.Map<ProductPrice>(command.ProductPrice);

            try
            {
                _database.ProductPricesHistory.Update(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"CQS Product price update error");
                throw;
            }

            var result = await _database.SaveChangesAsync(token);

            return result > 0;
        }
    }
}
