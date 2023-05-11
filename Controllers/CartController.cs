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
            var result = await _cartRepository.getProduct(uid);
            if (result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart(AddToCart cart)
        {
            var result = await _cartRepository.create(cart);
            if (result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("SubToCart")]
        public async Task<IActionResult> SubToCart(AddToCart cart)
        {
            var result = await _cartRepository.subItem(cart);
            if (result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("DeleteItem")]
        public async Task<IActionResult> RemoveItem(RemoveItemToCart remove)
        {
            var result = await _cartRepository.remove(remove);
            if (result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result);

        }

        [HttpDelete("DeleteAll")]
        public async Task<IActionResult> RemoveAllCart(int id)
        {
            var result = await _cartRepository.clearCart(id);
            if (result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result);

        }

    }
}
