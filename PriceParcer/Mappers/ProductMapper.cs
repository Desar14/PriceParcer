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

            CreateMap<ProductDTO, ProductDetailsViewModel>();

            CreateMap<MarketSite, MarketSiteInProductViewModel>();

            CreateMap<UserReview, UserReviewInProductViewModel>();

            CreateMap<UserReview, UserReviewInProductViewModel>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.User.UserName));

        }
    }
}
