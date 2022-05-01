using AutoMapper;
using MediatR;
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
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, bool>
    {
        private readonly ApplicationDbContext _database;
        private readonly IMapper _mapper;

        public AddProductCommandHandler(ApplicationDbContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<bool> Handle(AddProductCommand command, CancellationToken token)
        {
            var entity = _mapper.Map<Product>(command.Product);

            await _database.Products.AddAsync(entity, token);
            var result = await _database.SaveChangesAsync(token);

            return result > 0;
        }
    }
}
