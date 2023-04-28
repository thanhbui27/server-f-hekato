using DoAn.Repositories.ProductAction;
using DoAn.ViewModels.ProductAction;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductActionController : Controller
    {
        private readonly IProductActionRepositories _productActionRepositories;

        public ProductActionController(IProductActionRepositories productActionRepositories)
        {
            _productActionRepositories = productActionRepositories;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateProductAction create)
        {
            return Ok(await _productActionRepositories.Create(create));
        }
        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateProductAction update)
        {
            return Ok(await _productActionRepositories.Update(update));
        }

        [HttpDelete("remove")]
        public async Task<IActionResult> Delete(DeleteProductAction remove)
        {
            return Ok(await _productActionRepositories.Delete(remove));
        }
    }
}
