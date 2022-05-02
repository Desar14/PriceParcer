using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using PriceParser.Core.DTO;
using PriceParser.Data.Entities;
using PriceParser.Models;

namespace PriceParser.Mappers
{
    public class MarketSiteMapper : Profile
    {
        public MarketSiteMapper()
        {
            //CreateMap<MarketSite, MarketSiteInProductViewModel>();
            CreateMap<MarketSite, MarketSiteDTO>();

            CreateMap<MarketSiteDTO, MarketSiteListItemViewModel>()
                .ForMember(dest => dest.CreatedByUserName,
                    opt => opt.MapFrom(src => src.CreatedByUser.UserName));
            CreateMap<MarketSiteDTO, MarketSiteDetailsViewModel>();
            CreateMap<MarketSiteDTO, MarketSiteCreateEditViewModel>();
            CreateMap<MarketSiteDTO, MarketSiteDeleteViewModel>();

            CreateMap<MarketSiteDTO, MarketSite>();
            CreateMap<MarketSiteDTO, MarketSiteInProductViewModel>();
            CreateMap<MarketSiteDTO, SelectListItem>()
                .ForMember(dest => dest.Text,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Value,
                    opt => opt.MapFrom(src => src.Id));

            CreateMap<MarketSiteCreateEditViewModel, MarketSiteDTO>();

            CreateMap<ParseTypes, SelectListItem>()
                .ForMember(dest => dest.Text,
                    opt => opt.MapFrom(src => src.ToString()))
                .ForMember(dest => dest.Value,
                    opt => opt.MapFrom(src => ((int)src)));
        }
    }
}
