﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using PriceParser.Api.Models.Currency;
using PriceParser.Core.DTO;
using PriceParser.Data.Entities;

namespace PriceParser.Api.Mappers
{
    public class CurrencyMapper : Profile
    {
        public CurrencyMapper()
        {
            CreateMap<Currency, CurrencyDTO>();



            CreateMap<CurrencyDTO, SelectListItem>()
                .ForMember(dest => dest.Text,
                    opt => opt.MapFrom(src => src.Cur_Abbreviation))
                .ForMember(dest => dest.Value,
                    opt => opt.MapFrom(src => src.Id));


            CreateMap<CurrencyDTO, SelectListItem>()
                .ForMember(dest => dest.Text,
                    opt => opt.MapFrom(src => src.Cur_Abbreviation))
                .ForMember(dest => dest.Value,
                    opt => opt.MapFrom(src => src.Id));
            CreateMap<CurrencyDTO, GetCurrencyModel>();

            CreateMap<CurrencyRate, CurrencyRatesDTO>();


        }
    }
}
