using AutoMapper;
using PriceParcer.Core.DTO;
using PriceParcer.Core.Interfaces;
using PriceParcer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceParcer.Domain
{
    public class MarketSitesService : IMarketSitesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MarketSitesService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        async Task<bool> IMarketSitesService.AddSite(MarketSiteDTO product)
        {
            var entity = _mapper.Map<MarketSite>(product);

            await _unitOfWork.MarketSites.Add(entity);

            var result = await _unitOfWork.Commit();

            return result > 0;
        }

        async Task<bool> IMarketSitesService.DeleteSite(Guid id)
        {
            await _unitOfWork.MarketSites.Delete(id);

            var result = await _unitOfWork.Commit();

            return result > 0;
        }

        async Task<bool> IMarketSitesService.EditSite(MarketSiteDTO product)
        {
            var entity = _mapper.Map<MarketSite>(product);

            await _unitOfWork.MarketSites.Update(entity);

            var result = await _unitOfWork.Commit();

            return result > 0;
        }

        async Task<IEnumerable<MarketSiteDTO>> IMarketSitesService.GetAllSitesAsync()
        {
            return (await _unitOfWork.MarketSites.Get(includes: site => site.CreatedByUser))
                .Select(product => _mapper.Map<MarketSiteDTO>(product));
        }

        async Task<MarketSiteDTO> IMarketSitesService.GetSiteDetailsAsync(Guid id)
        {
            var result = (await _unitOfWork.MarketSites.GetByID(id));            

            return _mapper.Map<MarketSiteDTO>(result);
        }
    }
}
