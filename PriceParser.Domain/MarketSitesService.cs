using AutoMapper;
using Microsoft.Extensions.Logging;
using PriceParser.Core.DTO;
using PriceParser.Core.Interfaces;
using PriceParser.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceParser.Domain
{
    public class MarketSitesService : IMarketSitesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<MarketSitesService> _logger;

        public MarketSitesService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<MarketSitesService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<MarketSiteDTO>> GetOnlyAvailableSitesAsync()
        {
            return (await _unitOfWork.MarketSites.Get(filter: site => site.IsAvailable, includes: site => site.CreatedByUser))
               .Select(product => _mapper.Map<MarketSiteDTO>(product));
        }

        public async Task<bool> AddSite(MarketSiteDTO product)
        {
            var entity = _mapper.Map<MarketSite>(product);

            await _unitOfWork.MarketSites.Add(entity);

            var result = await _unitOfWork.Commit();

            return result > 0;
        }

        public async Task<bool> DeleteSite(Guid id)
        {
            await _unitOfWork.MarketSites.Delete(id);

            var result = await _unitOfWork.Commit();

            return result > 0;
        }

        public async Task<bool> EditSite(MarketSiteDTO product)
        {
            var entity = _mapper.Map<MarketSite>(product);

            await _unitOfWork.MarketSites.Update(entity);

            var result = await _unitOfWork.Commit();

            return result > 0;
        }

        public async Task<IEnumerable<MarketSiteDTO>> GetAllSitesAsync()
        {
            return (await _unitOfWork.MarketSites.Get(includes: site => site.CreatedByUser))
                .Select(product => _mapper.Map<MarketSiteDTO>(product));
        }

        public async Task<MarketSiteDTO> GetSiteDetailsAsync(Guid id)
        {
            var result = (await _unitOfWork.MarketSites.GetByID(id));            

            return _mapper.Map<MarketSiteDTO>(result);
        }
    }
}
