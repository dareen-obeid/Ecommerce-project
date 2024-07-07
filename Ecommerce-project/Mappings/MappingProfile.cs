using System;
using AutoMapper;
using Ecommerce_project.DTOs;
using Ecommerce_project.Models;

namespace Ecommerce_project.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<Product, ProductDto>()
           .ForMember(dto => dto.CategoryIds, opt => opt.MapFrom(p => p.ProductCategories.Select(pc => pc.CategoryId)));

            CreateMap<ProductDto, Product>()
                .ForMember(p => p.ProductCategories, opt => opt.Ignore());

            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();


            //CreateMap<Product, ProductDto>()
            //            .ForMember(dest => dest.CategoryIds,
            //                opt => opt.MapFrom(src => src.ProductCategories.Select(pc => pc.CategoryId).ToList()));
            //CreateMap<Category, CategoryDto>().ReverseMap();
            //CreateMap<ProductCategory, ProductCategoryDto>().ReverseMap();
        }
    }
}
