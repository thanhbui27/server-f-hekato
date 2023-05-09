using System.ComponentModel.DataAnnotations;

namespace DoAn.ViewModels.ProductImage
{
    public class ProductUploadImage
    {
        [Required]
        public int id { get; set; }

        [Required]
        public List<IFormFile> files { get; set; } 
    }
}
