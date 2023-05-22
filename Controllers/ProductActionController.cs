using DoAn.Repositories.ProductAction;
using DoAn.ViewModels.ProductAction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]
    public class ProductActionController : Controller
    {
        private readonly IProductActionRepositories _productActionRepositories;

        public ProductActionController(IProductActionRepositories productActionRepositories)
        {
            _productActionRepositories = productActionRepositories;
        }

        
        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateProductAction update)
        {
            var result = await _productActionRepositories.Update(update);
            if (result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result);

        }

        [HttpDelete("remove")]
        public async Task<IActionResult> Delete(DeleteProductAction remove)
        {
            var result = await _productActionRepositories.Delete(remove);
            if (result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
