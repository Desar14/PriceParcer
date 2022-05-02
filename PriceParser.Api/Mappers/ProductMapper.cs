using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using PriceParser.Api.Models.Product;
using PriceParser.Core.DTO;
using PriceParser.Data.Entities;


namespace PriceParser.Api.Mappers
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<Product, ProductDTO>();
            
            CreateMap<ProductDTO, Product>();
            
            CreateMap<ProductDTO, GetProductModel>()
                .ForMember(dest => dest.FromSites,
                    opt => opt.MapFrom(src => src.FromSites))
                .ForMember(dest => dest.Reviews,
                    opt => opt.MapFrom(src => src.Reviews));
            CreateMap<ProductDTO, SelectListItem>()
                .ForMember(dest => dest.Text,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Value,
                    opt => opt.MapFrom(src => src.Id));

            CreateMap<PostPutProductModel, ProductDTO>();



        }
    }
}
