using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            //CreateMap<Product, ProductItemListModel>();

            CreateMap<ProductDTO, Product>();
            CreateMap<ProductDTO, ProductItemListViewModel>();
            CreateMap<ProductDTO, ProductDetailsViewModel>()
                .ForMember(dest => dest.marketSites,
                    opt => opt.MapFrom(src => src.FromSites))
                .ForMember(dest => dest.userReviews,
                    opt => opt.MapFrom(src => src.Reviews));
            CreateMap<ProductDTO, ProductDeleteViewModel>();
            CreateMap<ProductDTO, ProductCreateEditViewModel>();
            CreateMap<ProductDTO, SelectListItem>()
                .ForMember(dest => dest.Text,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Value,
                    opt => opt.MapFrom(src => src.Id));

            CreateMap<ProductDeleteViewModel, ProductDTO>();
            
            CreateMap<ProductCreateEditViewModel, ProductDTO>();
        }
    }
}
