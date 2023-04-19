using System.ComponentModel.DataAnnotations;

namespace DoAn.ViewModels.Users
{
    public class UserLogin
    {
        [Required]
        public string userName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
