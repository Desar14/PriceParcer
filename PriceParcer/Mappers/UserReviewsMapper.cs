using AutoMapper;
using PriceParcer.Core.DTO;
using PriceParcer.Data;
using PriceParcer.Models;

namespace PriceParcer.Mappers
{
    public class UserReviewsMapper : Profile
    {
        public UserReviewsMapper()
        {
            CreateMap<UserReview, UserReviewDTO>();
            CreateMap<UserReview, UserReviewInProductViewModel>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.User.UserName));

            CreateMap<UserReviewDTO, UserReviewInProductViewModel>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.User.UserName));
        }
    }
}
