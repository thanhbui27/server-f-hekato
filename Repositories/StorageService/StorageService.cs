using DoAn.Repositories.StorageService.StorageService;
using Microsoft.AspNetCore.Hosting;

namespace DoAn.Repositories.StorageService
{
    public class StorageServices : IStorageService
    {
        private readonly string _userContentFolder;
        private const string USER_CONTENT_FOLDER_NAME = "Images";
        private readonly ILogger<StorageServices> _logger;
        public static IWebHostEnvironment _webHostEnvironment;
        public StorageServices(IWebHostEnvironment webHostEnvironment, ILogger<StorageServices> logger)
        {
            _webHostEnvironment = webHostEnvironment;

            _userContentFolder = Path.Combine("Resources", USER_CONTENT_FOLDER_NAME);

            _logger = logger;
        }

        public string GetFileUrl(string fileName)
        {
            return $"/{USER_CONTENT_FOLDER_NAME}/{fileName}";
        }

        public async Task SaveFileAsync(Stream mediaBinaryStream, string fileName)
        {

            var filePath = Path.Combine(_userContentFolder, fileName);
            using var output = new FileStream(filePath, FileMode.Create);
            await mediaBinaryStream.CopyToAsync(output);
        }

        public async Task DeleteFileAsync(string fileName)
        {
            var filePath = Path.Combine(_userContentFolder, fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }
    }
}
