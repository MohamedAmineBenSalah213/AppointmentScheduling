 using Microsoft.AspNetCore.Identity;

namespace AppointementScheduling.Models
{
    public class ApplicationUser :IdentityUser
    {
        public string Name { get; set; }
    }
}
