using AutoMapper;
using PriceParser.Api.Models.Prices;
using PriceParser.Api.Models.ProductFromSite;
using PriceParser.Core.DTO;
using PriceParser.Data.Entities;

namespace PriceParser.Api.Mappers
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

            CreateMap<ProductFromSitesDTO, GetProductFromSiteInfoInProductModel>()
                .ForMember(dest => dest.SiteName,
                        opt => opt.MapFrom(src => src.Site.Name))
                .ForMember(dest => dest.Id,
                        opt => opt.MapFrom(src => src.Id));

        }

    }
}
