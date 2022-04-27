using AutoMapper;
using PriceParser.Api.Models.Prices;
using PriceParser.Core.DTO;
using PriceParser.Data.Entities;

namespace PriceParser.Mappers
{
    public class ProductFromSitesMapper : Profile
    {

        public ProductFromSitesMapper()
        {
            CreateMap<ProductFromSites, ProductFromSitesDTO>();

            CreateMap<ProductFromSitesDTO, ProductFromSites>();
            
            CreateMap<ProductFromSitesDTO, GetPricesResponseModel>()
                .ForMember(dest => dest.SiteName,
                        opt => opt.MapFrom(src => src.Site.Name))
                .ForMember(dest => dest.ProductFromSiteId,
                        opt => opt.MapFrom(src => src.Id));

            
        }

    }
}
