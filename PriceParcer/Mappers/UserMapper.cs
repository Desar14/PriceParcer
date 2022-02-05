using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PriceParcer.Mappers
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<IdentityUser, SelectListItem>()
                .ForMember(dest => dest.Text,
                    opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Value,
                    opt => opt.MapFrom(src => src.Id));
        }
    }
}
