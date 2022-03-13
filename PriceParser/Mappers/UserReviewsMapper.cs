using AutoMapper;
using PriceParser.Core.DTO;
using PriceParser.Data;
using PriceParser.Models;
using PriceParser.Models.UserReview;

namespace PriceParser.Mappers
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
