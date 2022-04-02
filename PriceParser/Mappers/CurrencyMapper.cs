using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using PriceParser.Core.DTO;
using PriceParser.Data.Entities;
using PriceParser.Models;
using PriceParser.Models.Currency;

namespace PriceParser.Mappers
{
    public class CurrencyMapper : Profile
    {
        public CurrencyMapper()
        {
            CreateMap<Currency, CurrencyDTO>();

            CreateMap<CurrencyDTO, CurrencyListItemViewModel>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Periodicity,
                    opt => opt.MapFrom(src => src.Cur_Periodicity))
                .ForMember(dest => dest.Code,
                    opt => opt.MapFrom(src => src.Cur_Code))
                .ForMember(dest => dest.Abbreviation,
                    opt => opt.MapFrom(src => src.Cur_Abbreviation))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Cur_Name))
                .ForMember(dest => dest.Scale,
                    opt => opt.MapFrom(src => src.Cur_Scale));

            CreateMap<CurrencyDTO, CurrencyDetailsViewModel>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Periodicity,
                    opt => opt.MapFrom(src => src.Cur_Periodicity))
                .ForMember(dest => dest.Code,
                    opt => opt.MapFrom(src => src.Cur_Code))
                .ForMember(dest => dest.Abbreviation,
                    opt => opt.MapFrom(src => src.Cur_Abbreviation))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Cur_Name))
                .ForMember(dest => dest.Scale,
                    opt => opt.MapFrom(src => src.Cur_Scale));

            CreateMap < CurrencyDTO, CurrencyToggleUpdatingRatesModel>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Periodicity,
                    opt => opt.MapFrom(src => src.Cur_Periodicity))
                .ForMember(dest => dest.Code,
                    opt => opt.MapFrom(src => src.Cur_Code))
                .ForMember(dest => dest.Abbreviation,
                    opt => opt.MapFrom(src => src.Cur_Abbreviation))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Cur_Name))
                .ForMember(dest => dest.Scale,
                    opt => opt.MapFrom(src => src.Cur_Scale));

            CreateMap<CurrencyRate, CurrencyRatesDTO>();
            CreateMap<CurrencyRatesDTO, CurrencyRateListItemModel>()                
                .ForMember(dest => dest.Scale,
                    opt => opt.MapFrom(src => src.Cur_Scale))
                .ForMember(dest => dest.Date,
                    opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.OfficialRate,
                    opt => opt.MapFrom(src => src.Cur_OfficialRate));

        }
    }
}
