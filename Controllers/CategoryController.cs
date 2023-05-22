using DoAn.Repositories.Categorys;
using DoAn.Repositories.Users;
using DoAn.ViewModels.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]
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
            var result = await _cateRepositories.GetAll();
            if (result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("CreateCategory")]
        public async Task<IActionResult> Create(CategoryCreate cate)
        {
            var result = await _cateRepositories.Create(cate);
            if (result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result);

        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update(CategoryUpdate cate)
        {
            var result = await _cateRepositories.Update(cate);
            if (result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result);

        }

        [HttpDelete("Remove")]
        public async Task<IActionResult> Remove(CategoryRemove cate)
        {
            var result = await _cateRepositories.remove(cate);
            if (result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
