using AutoMapper;
using PriceParcer.Core.DTO;
using PriceParcer.Data;
using PriceParcer.Models;

namespace PriceParcer.Mappers
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<Product, ProductDTO>();
            CreateMap<Product, ProductItemListModel>();
            CreateMap<ProductDTO, Product>();
        }
    }
}
