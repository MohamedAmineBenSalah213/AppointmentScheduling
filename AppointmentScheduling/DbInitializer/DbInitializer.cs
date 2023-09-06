using AppointementScheduling.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AppointementScheduling.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDBcontext dBcontext;
        private readonly UserManager<ApplicationUser> user;
        private readonly RoleManager<IdentityRole> role;
        public DbInitializer(ApplicationDBcontext application,UserManager<ApplicationUser> manager,RoleManager<IdentityRole> roleManager)
        {
               dBcontext = application; 
            user = manager;
            role = roleManager;
        }
        public  void Initialize()
        {
            try
            {
                if (dBcontext.Database.GetPendingMigrations().Count() > 0)
                {
                    dBcontext.Database.Migrate();
                }
            }catch(Exception e)
            {

            }
            if (dBcontext.Roles.Any(x => x.Name == Helper.Helper.admin))
            {
                return;
            }
           
                 role.CreateAsync(new IdentityRole(Helper.Helper.doctor)).GetAwaiter().GetResult();
                 role.CreateAsync(new IdentityRole(Helper.Helper.admin)).GetAwaiter().GetResult();
                 role.CreateAsync(new IdentityRole(Helper.Helper.patient)).GetAwaiter().GetResult();
            user.CreateAsync(new ApplicationUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                Name = "adminstration "

            }, "Admin123").GetAwaiter().GetResult();
            ApplicationUser appuser = dBcontext.Users.FirstOrDefault(u => u.Email == "admin@gmail.com");
            user.AddToRoleAsync(appuser, Helper.Helper.admin).GetAwaiter().GetResult();
           
        }
    }
}
