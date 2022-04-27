using AutoMapper;
using PriceParser.Core.DTO;
using PriceParser.Data.Entities;

namespace PriceParser.Api.Mappers
{
    public class MarketSiteMapper : Profile
    {
        public MarketSiteMapper()
        {
            CreateMap<MarketSite, MarketSiteDTO>();
            CreateMap<MarketSiteDTO, MarketSite>();
        }
    }
}
