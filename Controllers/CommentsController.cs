using DoAn.Models;
using DoAn.Repositories.Comment;
using DoAn.ViewModels.Comments;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : Controller
    {
        private readonly ICommentsRepositories _commentsRepositories;
        
        public CommentsController(ICommentsRepositories commentsRepositories) {
            _commentsRepositories= commentsRepositories;
        }

        [HttpGet("getAllComment")]
        public async Task<IActionResult> getAllComment(int productId)
        {
            var result = await _commentsRepositories.getComment(productId);
            if (result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result);

        }

        [HttpPost("create")]
        public async Task<IActionResult> create(CreateComments create)
        {
            var result = await _commentsRepositories.create(create);
            if (result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> remove(int id)
        {
            var result = await _commentsRepositories.delete(id);
            if (result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


    }
}
