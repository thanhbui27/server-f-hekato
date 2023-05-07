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
            return Ok(await _commentsRepositories.getComment(productId));
        }

        [HttpPost("create")]
        public async Task<IActionResult> create(CreateComments create)
        {
            return Ok(await _commentsRepositories.create(create));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> remove(int id)
        {
            return Ok(await _commentsRepositories.delete(id));
        }


    }
}
