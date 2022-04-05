using AutoMapper;
using PriceParser.Core.DTO;
using PriceParser.Data.Entities;
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
                    opt => opt.MapFrom(src => src.ParseDate.Date))
                .ForMember(dest => dest.Price,
                    opt => opt.MapFrom(src => src.FullPrice));

        }
    }
}
