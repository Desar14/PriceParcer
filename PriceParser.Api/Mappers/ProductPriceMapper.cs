using AutoMapper;
using PriceParser.Api.Models.Prices;
using PriceParser.Core.DTO;
using PriceParser.Data.Entities;

namespace PriceParser.Api.Mappers
{
    public class ProductPriceMapper : Profile
    {
        public ProductPriceMapper()
        {
            CreateMap<ProductPrice, ProductPriceDTO>();

            CreateMap<ProductPriceDTO, ProductPrice>();

            CreateMap<ProductPriceDTO, ProductPriceDataItem>()
                .ForMember(dest => dest.Date,
                    opt => opt.MapFrom(src => src.ParseDate))
                .ForMember(dest => dest.Price,
                    opt => opt.MapFrom(src => src.FullPrice));

        }
    }
}
