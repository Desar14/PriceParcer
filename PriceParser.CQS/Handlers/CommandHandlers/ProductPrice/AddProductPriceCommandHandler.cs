using AutoMapper;
using MediatR;
using PriceParser.CQS.Models.Commands;
using PriceParser.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceParser.CQS.Handlers.CommandHandlers
{
    public class AddProductPriceCommandHandler : IRequestHandler<AddProductPriceCommand, bool>
    {
        private readonly ApplicationDbContext _database;
        private readonly IMapper _mapper;

        public AddProductPriceCommandHandler(ApplicationDbContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<bool> Handle(AddProductPriceCommand command, CancellationToken token)
        {
            var entity = _mapper.Map<ProductPrice>(command.ProductPrice);

            await _database.ProductPricesHistory.AddAsync(entity, token);
            var result = await _database.SaveChangesAsync(token);           

            return result > 0;            
        }
    }
}
