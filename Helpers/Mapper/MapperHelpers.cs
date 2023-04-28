﻿using AutoMapper;
using DoAn.Models;
using DoAn.ViewModels.Category;
using DoAn.ViewModels.Product;
using DoAn.ViewModels.ProductAction;
using DoAn.ViewModels.ProductImage;

namespace DoAn.Helpers.Mapper
{
    public class MapperHelpers : Profile
    {
        public MapperHelpers() {
            //product action
            CreateMap<ProductActions, CreateProductAction>().ReverseMap();
            CreateMap<ProductActions, UpdateProductAction>().ReverseMap();
            CreateMap<ProductActions, DeleteProductAction>().ReverseMap();

            //category
            CreateMap<Category, CategoryCreate>().ReverseMap();
            CreateMap<Category, CategoryGetAll>().ReverseMap();
            CreateMap<Category, CategoryRemove>().ReverseMap();
            CreateMap<Category, CategoryUpdate>().ReverseMap();

            //product
            CreateMap<Product, ProductCreate>().ReverseMap();
            CreateMap<Product, ProductGetAll>().ReverseMap();
            CreateMap<Product, ProductGetById>().ReverseMap();

            //product image
            CreateMap<ProductImage, GetProductImage>().ReverseMap();

            CreateMap<ProductInCategory, ProductRemoveCatgory>().ReverseMap();
        }
    }
}
