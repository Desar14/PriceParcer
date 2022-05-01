using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PriceParser.CQS.Models.Commands;
using PriceParser.Data;
using PriceParser.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceParser.CQS.Handlers.CommandHandlers
{
    public class EditProductCommandHandler : IRequestHandler<EditProductCommand, bool>
    {
        protected readonly ApplicationDbContext _database;
        protected readonly IMapper _mapper;
        protected readonly ILogger<EditProductCommandHandler> _logger;

        public EditProductCommandHandler(ApplicationDbContext database, IMapper mapper, ILogger<EditProductCommandHandler> logger)
        {
            _database = database;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> Handle(EditProductCommand command, CancellationToken token)
        {
            var entity = _mapper.Map<Product>(command.Product);

            try
            {
                _database.Products.Update(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"CQS Product update error");
                throw;
            }

            var result = await _database.SaveChangesAsync(token);

            return result > 0;
        }
    }
}
