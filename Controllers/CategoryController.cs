using DoAn.Repositories.Categorys;
using DoAn.Repositories.Users;
using DoAn.ViewModels.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class CategoryController : Controller
    {
        public readonly ICategoryRepositories _cateRepositories;
        public CategoryController(ICategoryRepositories cateRepositories)
        {
            _cateRepositories = cateRepositories;
        }

        [HttpGet("GetAllCategory")]
        public async Task<IActionResult> GetAll()
        {
            return Ok( await _cateRepositories.GetAll());
        }

        [HttpPost("CreateCategory")]
        public async Task<IActionResult> Create(CategoryCreate cate)
        {
            if(cate.CategoryName == null)
            {
                return BadRequest();
            }

            return Ok(await _cateRepositories.Create(cate));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update(CategoryUpdate cate)
        {
            if(cate != null)
            {
                return Ok(await _cateRepositories.Update(cate));
            }
            return BadRequest();
        }

        [HttpDelete("Remove")]
        public async Task<IActionResult> Remove(CategoryRemove cate)
        {
            if(cate != null)
            {
                return Ok(await _cateRepositories.remove(cate));
            }
            return BadRequest();
        }
    }
}
