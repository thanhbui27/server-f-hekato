using AutoMapper;
using DoAn.Models;
using DoAn.ViewModels.Cart;
using DoAn.ViewModels.Category;
using DoAn.ViewModels.Comments;
using DoAn.ViewModels.Orders;
using DoAn.ViewModels.Product;
using DoAn.ViewModels.ProductAction;
using DoAn.ViewModels.ProductImage;
using DoAn.ViewModels.Users;
using System.Security.Cryptography.X509Certificates;

namespace DoAn.Helpers.Mapper
{
    public class MapperHelpers : Profile
    {
        public MapperHelpers() {

            //user
            CreateMap<UserRegister, UserModels>().ReverseMap();

            //comment
            CreateMap<CommentsProducts , CreateComments>().ReverseMap();

            //order

            CreateMap<Orders, CreateOrders>().ReverseMap();

            //cart
            CreateMap<Cart, AddToCart>().ReverseMap();
            CreateMap<Cart, GetProductToCart>().ReverseMap();
            CreateMap<Cart, RemoveItemToCart>().ReverseMap();
            CreateMap<Cart, UpdateToCart>().ReverseMap();

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
