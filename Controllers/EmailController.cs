using DoAn.Repositories.Email;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.Controllers
{
    public class EmailController : Controller
    {
        private readonly IEmailRepositories _emailRepositories;

        public EmailController(IEmailRepositories emailRepositories)
        {
            _emailRepositories = emailRepositories;
        }

        [HttpGet("sendEmail")]
        public IActionResult SendEmail(string emailName, string title, string content)
        {
            var result = _emailRepositories.SendEmail(emailName, title, content);
            return Ok(result);
        }
    }
}
