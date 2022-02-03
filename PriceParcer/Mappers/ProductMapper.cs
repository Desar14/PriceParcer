using AutoMapper;
using PriceParcer.Core.DTO;
using PriceParcer.Data;
using PriceParcer.Models;

namespace PriceParcer.Mappers
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<Product, ProductDTO>();
            CreateMap<Product, ProductItemListModel>();
            CreateMap<ProductDTO, Product>();
            CreateMap<ProductDTO, ProductItemListModel>();              

            CreateMap<ProductFromSitesDTO, MarketSiteInProductViewModel>();
            CreateMap<ProductFromSitesDTO, MarketSiteInProductViewModel>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Site.Name));

            CreateMap<MarketSite, MarketSiteInProductViewModel>();

            CreateMap<MarketSiteDTO, MarketSiteListItemViewModel>()
                .ForMember(dest => dest.CreatedByUserName,
                    opt => opt.MapFrom(src => src.CreatedByUser.UserName));
            CreateMap<MarketSiteDTO, MarketSiteDetailsViewModel>();
            CreateMap<MarketSiteDTO, MarketSitesCreateEditViewModel>();
            CreateMap<MarketSitesCreateEditViewModel, MarketSiteDTO>();

            CreateMap<UserReview, UserReviewInProductViewModel>();
            CreateMap<UserReview, UserReviewDTO>();
            CreateMap<UserReviewDTO, UserReviewInProductViewModel>();


            CreateMap<MarketSite, MarketSiteDTO>();
            CreateMap<MarketSiteDTO, MarketSite>();
            CreateMap<MarketSiteDTO, MarketSiteInProductViewModel>();

            CreateMap<UserReview, UserReviewInProductViewModel>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.User.UserName));

            CreateMap<ProductFromSites, ProductFromSitesDTO>();

            CreateMap<ProductDTO, ProductDetailsViewModel>();
            CreateMap<ProductDTO, ProductDetailsViewModel>()
                .ForMember(dest => dest.marketSites,
                    opt => opt.MapFrom(src => src.FromSites))
                .ForMember(dest => dest.userReviews,
                    opt => opt.MapFrom(src => src.Reviews));

            CreateMap<CreateEditProductViewModel, ProductDTO>();
            CreateMap<ProductDTO, CreateEditProductViewModel>();

            CreateMap<ProductDeleteViewModel, ProductDTO>();
            CreateMap<ProductDTO, ProductDeleteViewModel>();
        }
    }
}
