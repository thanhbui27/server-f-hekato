using Microsoft.AspNetCore.Identity;

namespace DoAn.Models
{
    public class UserModels : IdentityUser<Guid>
    {
        public string firstName { get; set; }
        public string lastName { get; set; }

        public DateTime? dob { get; set; } 
    }
}
