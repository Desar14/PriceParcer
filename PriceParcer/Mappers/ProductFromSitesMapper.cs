using AutoMapper;
using PriceParcer.Core.DTO;
using PriceParcer.Data;
using PriceParcer.Models;
using PriceParcer.Models.ProductFromSite;

namespace PriceParcer.Mappers
{
    public class ProductFromSitesMapper : Profile
    {

        public ProductFromSitesMapper()
        {
            CreateMap<ProductFromSites, ProductFromSitesDTO>();

            CreateMap<ProductFromSitesDTO, ProductFromSites>();
            CreateMap<ProductFromSitesDTO, MarketSiteInProductViewModel>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Site.Name));

            CreateMap<ProductFromSitesDTO, ProductFromSiteItemListViewModel>()
                .ForMember(dest => dest.SiteName,
                    opt => opt.MapFrom(src => src.Site.Name))
                .ForMember(dest => dest.ProductName,
                    opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.CreatedByUserName,
                    opt => opt.MapFrom(src => src.CreatedByUser.UserName));

            CreateMap<ProductFromSitesDTO, ProductFromSiteDetailsViewModel>()
                    .ForMember(dest => dest.SiteName,
                        opt => opt.MapFrom(src => src.Site.Name))
                    .ForMember(dest => dest.ProductName,
                        opt => opt.MapFrom(src => src.Product.Name))
                    .ForMember(dest => dest.CreatedByUserName,
                        opt => opt.MapFrom(src => src.CreatedByUser.UserName));

            CreateMap<ProductFromSitesDTO, ProductFromSiteCreateEditViewModel>();
            CreateMap<ProductFromSitesDTO, ProductFromSiteDeleteViewModel>();

            CreateMap<ProductFromSiteCreateEditViewModel, ProductFromSitesDTO>();

            CreateMap<ProductFromSiteDeleteViewModel, ProductFromSitesDTO>();
        }

    }
}
