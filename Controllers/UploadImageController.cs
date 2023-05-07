using DoAn.Repositories.StorageService.StorageService;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace DoAn.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadImageController : Controller
    {

        private readonly IStorageService _storageService;
        private const string USER_CONTENT_FOLDER_NAME = "Images";

        public UploadImageController(IStorageService storageService)
        {
            _storageService = storageService;
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return "/" + USER_CONTENT_FOLDER_NAME + "/" + fileName;
        }

        [HttpPost("upload")]
        [Produces("application/json")]
        public async Task<IActionResult> upload()
        {
            var filess = Request.Form.Files;
            var theFile = filess.FirstOrDefault();
            return Ok(await this.SaveFile(theFile));
        }
    }
}
