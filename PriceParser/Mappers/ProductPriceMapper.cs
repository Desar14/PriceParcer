using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using PriceParser.Core.DTO;
using PriceParser.Data;
using PriceParser.Models;
using PriceParser.Models.ProductPrice;

namespace PriceParser.Mappers
{
    public class ProductPriceMapper : Profile
    {
        public ProductPriceMapper()
        {
            CreateMap<ProductPrice, ProductPriceDTO>();

            CreateMap<ProductPriceDTO, ProductPrice>();

            CreateMap<ProductPriceDTO, ProductPriceItemListViewModel>();

            CreateMap<ProductPriceDTO, ProductPriceDataItem>()
                .ForMember(dest => dest.Date,
                    opt => opt.MapFrom(src => src.ParseDate.Date));

        }
    }
}
