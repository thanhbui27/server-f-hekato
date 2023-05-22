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
            var result = await _productRepositories.GetProductFeature();
            if (result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpGet("GetProductNewArrival")]
        public async Task<IActionResult> GetProductNewArrival()
        {
            var result = await _productRepositories.GetProductNewArrival();
            if (result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result);
 
        }


        [HttpGet("GetProductBestSeller")]
        public async Task<IActionResult> GetProductBestSeller()
        {
            var result = await _productRepositories.GetProductBestSeller();
            if (result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpGet("GetProductSpecialOffer")]
        public async Task<IActionResult> GetProductSpecialOffer()
        {
            var result = await _productRepositories.GetProductSpecialOffer();
            if (result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result);
   
        }

        [HttpGet("GetProductTrending")]
        public async Task<IActionResult> GetProductTrending()
        {
            var result = await _productRepositories.GetProductTrending();
            if (result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetProductTrendSmall")]
        public async Task<IActionResult> GetProductTrendSmall()
        {
            var result = await _productRepositories.GetProductTrendSmall();
            if (result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _productRepositories.GetById(id);
            if (result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result);

        }

        [HttpGet("GetImageById")]

        public async Task<IActionResult> GetImageById(int id)
        {
            var result = await _productRepositories.GetAllImageById(id);
            if (result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result);
 
        }

        [Authorize(Roles = "admin")]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] ProductCreate create)
        {
            if (create != null)
            {
                var result = await _productRepositories.create(create);
                if (result.IsSuccessed)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return BadRequest();
        }

        [Authorize(Roles = "admin")]
        [HttpPost("UploadImage")]

        public async Task<IActionResult> UploadImage([FromForm]ProductUploadImage upload)
        {
            if (upload != null)
            {
                var result = await _productRepositories.UploadImage(upload.id, upload.files);
                if (result.IsSuccessed)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return BadRequest();
        }


        [Authorize(Roles = "admin")]
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromForm] ProductUpdate update)
        {
            if(update != null)
            {
                var result = await _productRepositories.update(update);
                if (result.IsSuccessed)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return BadRequest();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(ProductDelete dele)
        {
            if(dele != null)
            {
                var result = await _productRepositories.delete(dele);
                if (result.IsSuccessed)
                {
                    return Ok(result);
                }
                return BadRequest(result);
              
            }

            return BadRequest();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("DeleteCategory")]

        public async Task<IActionResult> DeleteCcategory(ProductRemoveCatgory rm)
        {
            if (rm != null)
            {
                var result = await _productRepositories.RemoveCategory(rm);
                if (result.IsSuccessed)
                {
                    return Ok(result);
                }
                return BadRequest(result);
               
            }

            return BadRequest();
        }


    }
}
