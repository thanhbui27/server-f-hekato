using DoAn.Models;
using DoAn.Repositories.Carts;
using DoAn.ViewModels.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : Controller
    {

        private readonly ICartRepositories _cartRepository;

        public CartController(ICartRepositories cartRepository)
        {
            _cartRepository = cartRepository;
        }

        [HttpGet("GetCartByUId")]
        public async Task<IActionResult> GetCartByUid(int uid)
        {
            return Ok(await _cartRepository.getProduct(uid));
        }

        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart(AddToCart cart)
        {
            return Ok(await _cartRepository.create(cart));
        }

        [HttpPost("SubToCart")]
        public async Task<IActionResult> SubToCart(AddToCart cart)
        {
            return Ok(await _cartRepository.subItem(cart));
        }

        [HttpDelete("DeleteItem")]
        public async Task<IActionResult> RemoveItem(RemoveItemToCart remove)
        {
            return Ok(await _cartRepository.remove(remove));
        }

        [HttpDelete("DeleteAll")]
        public async Task<IActionResult> RemoveAllCart(int id)
        {
            return Ok(await _cartRepository.clearCart(id));    

        }

    }
}
