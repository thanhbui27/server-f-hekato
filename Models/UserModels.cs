using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

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

        public List<Orders> orders { get; set; }

    }
}
