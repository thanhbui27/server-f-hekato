using AutoMapper;
using DoAn.EF;
using DoAn.Helpers.ApiResponse;
using DoAn.Models;
using DoAn.ViewModels.Cart;
using DoAn.ViewModels.Category;
using DoAn.ViewModels.Product;
using DoAn.ViewModels.ProductImage;

namespace DoAn.Repositories.Carts
{
    public class CartRepositories : ICartRepositories
    {
        private readonly EFDbContext _context;
        private readonly IMapper _mapper;

        public CartRepositories(EFDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResult<bool>> clearCart(int id)
        {
            var cart = _context.carts.Where(x => x.SessionId == id).ToList();

            foreach(Cart c in cart)
            {
                _context.carts.Remove(c);
                await _context.SaveChangesAsync();
            }
            return new ApiSuccessResult<bool>{
                IsSuccessed = true,
                Message = "Xoá giỏ hàng thành công"
            };
        }

        public async Task<ApiResult<bool>> create(AddToCart cart)
        {

            var _cart = _context.carts.Where(x => x.SessionId == cart.SessionId).Where(x => x.ProductId == cart.ProductId).FirstOrDefault();

            if (_cart == null)
            {
                var Acart = _mapper.Map<Cart>(cart);
                _context.carts.Add(Acart);

                await _context.SaveChangesAsync();
                return new ApiSuccessResult<bool>
                {
                    IsSuccessed = true,
                    Message = "Thêm mới sản phẩm vào giỏ hàng thành công"
                };
            }
            else
            {
                _cart.quantity = _cart.quantity + 1;

                _context.carts.Update(_cart);

                await _context.SaveChangesAsync();
                return new ApiSuccessResult<bool>
                {
                    IsSuccessed = true,
                    Message = "Thêm giỏ hàng thành công"
                };
            }

        }

        public async Task<ApiResult<List<GetProductToCart>>> getProduct(int uid)
        {
            var cart = _context.carts.Select(x => new GetProductToCart
            {
                Id = x.Id,
                SessionId = x.SessionId,
                quantity = x.quantity,
                productGetAll = _mapper.Map<ProductGetAll>(_context.products.Select(p => new ProductGetAll
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Image_Url = p.Image_Url,
                    quantity = p.quantity,
                    PriceNew = p.PriceNew,
                    PriceOld = p.PriceOld,
                    ShortDetails = p.ShortDetails,
                    ProductDescription = p.ProductDescription,
                    dateAdd = p.dateAdd,
                    Categories = p.GetsProductInCategories
                        .Select(pc => pc.GetCategory)
                        .Select(c => new CategoryGetAll
                        {
                            CategoryId = c.CategoryId,
                            CategoryName = c.CategoryName
                        }).ToList()
                }).FirstOrDefault(pt => pt.ProductId == x.ProductId))
            }).Where(x => x.SessionId == uid).ToList();

            return new ApiSuccessResult<List<GetProductToCart>>(cart);
        }

        public async Task<ApiResult<bool>> remove(RemoveItemToCart remove)
        {
            var _cart = _mapper.Map<Cart>(remove);
            _context.carts.Remove(_cart);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>{ IsSuccessed = true, Message = "Xoá giỏ hàng thành công" };

        }

        public async Task<ApiResult<bool>> subItem(AddToCart cart)
        {
            var _cart = _context.carts.Where(x => x.SessionId == cart.SessionId).Where(x => x.ProductId == cart.ProductId).FirstOrDefault();

            _cart.quantity = cart.quantity - 1;

            _context.carts.Update(_cart);

            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>{ IsSuccessed = true, Message = "xoá sản phẩm trong giỏ hàng thành công" };
        }
    }
}
