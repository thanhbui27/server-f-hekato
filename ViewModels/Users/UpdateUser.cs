using System.ComponentModel.DataAnnotations;

namespace DoAn.ViewModels.Users
{
    public class UpdateUser
    {
        public string uid { get; set; }
        public string? fullName { get; set; }
        public string? address { get; set; }

        public string? phoneNumber { get; set; }
        public string? email { get; set; }

        public DateTime? dob { get; set; }

        public string? cmnd { get; set; }
    }
}
