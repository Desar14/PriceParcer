using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PriceParser.CQS.Models.Commands;

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
            var entity = await _database.Products.FirstOrDefaultAsync(x => x.Id == command.Id, token);
            if (entity == null)
                throw new Exception();
            else
                _database.Products.Remove(entity);

            var result = await _database.SaveChangesAsync(token);

            return result > 0;
        }
    }
}
