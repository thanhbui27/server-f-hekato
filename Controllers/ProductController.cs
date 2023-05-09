using DoAn.Repositories.Products;
using DoAn.ViewModels.Product;
using DoAn.ViewModels.ProductImage;
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

        [HttpGet("GetProductFeature")]
        public async Task<IActionResult> GetProductFeature()
        {
            return Ok(await _productRepositories.GetProductFeature());
        }


        [HttpGet("GetProductNewArrival")]
        public async Task<IActionResult> GetProductNewArrival()
        {
            return Ok(await _productRepositories.GetProductNewArrival());
        }


        [HttpGet("GetProductBestSeller")]
        public async Task<IActionResult> GetProductBestSeller()
        {
            return Ok(await _productRepositories.GetProductBestSeller());
        }


        [HttpGet("GetProductSpecialOffer")]
        public async Task<IActionResult> GetProductSpecialOffer()
        {
            return Ok(await _productRepositories.GetProductSpecialOffer());
        }

        [HttpGet("GetProductTrending")]
        public async Task<IActionResult> GetProductTrending()
        {
            return Ok(await _productRepositories.GetProductTrending());
        }

        [HttpGet("GetProductTrendSmall")]
        public async Task<IActionResult> GetProductTrendSmall()
        {
            return Ok(await _productRepositories.GetProductTrendSmall());
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

        public async Task<IActionResult> UploadImage([FromForm]ProductUploadImage upload)
        {
            if (upload != null)
            {
                return Ok(await _productRepositories.UploadImage(upload.id, upload.files));
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
