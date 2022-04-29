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
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly ApplicationDbContext _database;
        private readonly IMapper _mapper;

        public DeleteProductCommandHandler(ApplicationDbContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<bool> Handle(DeleteProductCommand command, CancellationToken token)
        {
            var entity = await _database.Products.FindAsync(command.Id, token);
            if (entity == null)
                throw new Exception();
            else
                 _database.Products.Remove(entity);
            
            var result = await _database.SaveChangesAsync(token);           

            return result > 0;            
        }
    }
}
