using Microsoft.AspNetCore.Identity;

namespace DoAn.Models
{
    public class UserModels : IdentityUser<Guid>
    {
        public string fullName { get; set; }

        public string type { get; set; }
        public DateTime? dob { get; set; } 
    }
}
