using AutoMapper;
using PriceParcer.Core.DTO;
using PriceParcer.Data;
using PriceParcer.Models;
using PriceParcer.Models.UserReview;

namespace PriceParcer.Mappers
{
    public class UserReviewsMapper : Profile
    {
        public UserReviewsMapper()
        {
            CreateMap<UserReview, UserReviewDTO>();

            CreateMap<UserReviewDTO, UserReview>();
            CreateMap<UserReviewDTO, UserReviewInProductViewModel>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.User.UserName));
            CreateMap<UserReviewDTO, UserReviewItemListViewModel>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.ProductName,
                    opt => opt.MapFrom(src => src.Product.Name));
            CreateMap<UserReviewDTO, UserReviewDetailsViewModel>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.ProductName,
                    opt => opt.MapFrom(src => src.Product.Name));
            CreateMap<UserReviewDTO, UserReviewDeleteViewModel>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.ProductName,
                    opt => opt.MapFrom(src => src.Product.Name));
            CreateMap<UserReviewDTO, UserReviewCreateEditViewModel>();

            CreateMap<UserReviewCreateEditViewModel, UserReviewDTO>();

            CreateMap<UserReviewDeleteViewModel, UserReviewDTO>();
        }
    }
}
