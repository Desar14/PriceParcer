using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using PriceParser.Data.Entities;

namespace PriceParser.Mappers
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<ApplicationUser, SelectListItem>()
                .ForMember(dest => dest.Text,
                    opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Value,
                    opt => opt.MapFrom(src => src.Id));
        }
    }
}
