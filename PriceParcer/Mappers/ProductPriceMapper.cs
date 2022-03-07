using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using PriceParcer.Core.DTO;
using PriceParcer.Data;
using PriceParcer.Models;
using PriceParcer.Models.ProductPrice;

namespace PriceParcer.Mappers
{
    public class ProductPriceMapper : Profile
    {
        public ProductPriceMapper()
        {
            CreateMap<ProductPrice, ProductPriceDTO>();

            CreateMap<ProductPriceDTO, ProductPrice>();

            CreateMap<ProductPriceDTO, ProductPriceItemListViewModel>();

        }
    }
}
