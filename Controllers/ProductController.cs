using DoAn.Repositories.Products;
using DoAn.ViewModels.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DoAn.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductRepositories _productRepositories;
        private readonly ILogger<ProductController> _logger;
       
        public ProductController(IProductRepositories productRepositories, ILogger<ProductController> logger)
        {
            _productRepositories = productRepositories;
            _logger = logger;
        }



        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery]GetProductRequestPagi request) {
            return Ok(await _productRepositories.GetAll(request));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _productRepositories.GetById(id));
        }

        [HttpGet("GetImageById")]

        public async Task<IActionResult> GetImageById(int id)
        {
            return Ok(await _productRepositories.GetAllImageById(id));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] ProductCreate create)
        {
            if (create != null)
            {
                //
                return Ok(await _productRepositories.create(create));
            }
            return BadRequest();
        }

        [HttpPost("UploadImage")]

        public async Task<IActionResult> UploadImage([FromQuery] int id, [FromQuery] List<IFormFile> list)
        {
            if(list != null)
            {
                return Ok(await _productRepositories.UploadImage(id, list));
            }
            return BadRequest();
        }



        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromForm] ProductUpdate update)
        {
            if(update != null)
            {
                return Ok(await _productRepositories.update(update));
            }
            return BadRequest();
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(ProductDelete dele)
        {
            if(dele != null)
            {
                var result = await _productRepositories.delete(dele);
                return Ok(result);
            }

            return BadRequest();
        }

        [HttpDelete("DeleteCategory")]

        public async Task<IActionResult> DeleteCcategory(ProductRemoveCatgory rm)
        {
            if (rm != null)
            {
                var result = await _productRepositories.RemoveCategory(rm);
                return Ok(result);
            }

            return BadRequest();
        }


    }
}
