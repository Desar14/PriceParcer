using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PriceParser.Core.DTO;
using PriceParser.CQS.Handlers.CommandHandlers;
using PriceParser.CQS.Models.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceParser.CQS.Handlers.QueriesHandlers
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDTO>>
    {
        protected readonly ApplicationDbContext _database;
        protected readonly IMapper _mapper;
        protected readonly ILogger<EditProductCommandHandler> _logger;

        public GetAllProductsQueryHandler(ApplicationDbContext database, IMapper mapper, ILogger<EditProductCommandHandler> logger)
        {
            _database = database;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductDTO>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {            
            return await _database.Products.Select(product => _mapper.Map<ProductDTO>(product)).ToListAsync(cancellationToken);
        }
    }
}
