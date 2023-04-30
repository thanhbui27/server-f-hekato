using Microsoft.AspNetCore.Identity;

namespace DoAn.Models
{
    public class UserModels : IdentityUser<Guid>
    {
        public string fullName { get; set; }

        public string type { get; set; }
        public DateTime? dob { get; set; } 

        public string? address { get; set; }

        public string? CMND { get; set; } 
        public Session session { get; set; }
    }
}
