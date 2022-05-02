using AutoMapper;
using PriceParser.Api.Models.UserReview;
using PriceParser.Core.DTO;
using PriceParser.Data.Entities;

namespace PriceParser.Mappers
{
    public class UserReviewsMapper : Profile
    {
        public UserReviewsMapper()
        {
            CreateMap<UserReview, UserReviewDTO>();

            CreateMap<UserReviewDTO, UserReview>();
            CreateMap<UserReviewDTO, GetUserReviewInProductModel>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.User.UserName));
            CreateMap<UserReviewDTO, GetUserReviewModel>();

            CreateMap<PostUserReviewModel, UserReviewDTO>();
        }
    }
}
