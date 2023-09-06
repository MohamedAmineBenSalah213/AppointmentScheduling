using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppointementScheduling.Models
{
    public class ApplicationDBcontext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBcontext(DbContextOptions<ApplicationDBcontext> options):base(options)
        {

        }
        public DbSet<Appointment> Appointments { get; set; }
    }
}
