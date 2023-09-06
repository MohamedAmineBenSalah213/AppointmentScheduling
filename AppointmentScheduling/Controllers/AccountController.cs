using AppointementScheduling.Models;
using AppointementScheduling.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppointementScheduling.Controllers
{
    public class AccountController : Controller
    { private readonly ApplicationDBcontext dbcontext;
        UserManager<ApplicationUser> UserManger;
        SignInManager<ApplicationUser> SignIn;
        RoleManager<IdentityRole> RoleManger;
        public AccountController(ApplicationDBcontext _dbcontext, UserManager<ApplicationUser> userManger, SignInManager<ApplicationUser> signIn, RoleManager<IdentityRole> roleManger)
        {
            dbcontext = _dbcontext;
            UserManger = userManger;
            SignIn = signIn;
            RoleManger = roleManger;    
        }
        public IActionResult Login()
        {
            return View(); 
        }
        //implementation "Login"
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            //server side validation
            if (ModelState.IsValid)
            {
              
                var user = await SignIn.PasswordSignInAsync(model.Email, model.Password, model.RememberMe,false);
                if (user.Succeeded)
                { //session variable
                   var  usersession = await UserManger.FindByNameAsync(model.Email);
                    HttpContext.Session.SetString("ssuserName", usersession.Name);
                    //retrive data 
                    //  HttpContext.Session.GetString("ssuserName", usersession.Name);
                    return RedirectToAction("Index", "Appointement");
                }
             
                else
                {
                    //sinon
                    ModelState.AddModelError("", "Invalid login attempt");                                                  
                }
              
            }
            return View(model);
          
        }
        public async Task<IActionResult> Register()
        {
            //verificattion des roles
            if (!RoleManger.RoleExistsAsync(Helper.Helper.admin).GetAwaiter().GetResult()){
               await RoleManger.CreateAsync(new IdentityRole(Helper.Helper.doctor));
               await RoleManger.CreateAsync(new IdentityRole(Helper.Helper.admin));
               await RoleManger.CreateAsync(new IdentityRole(Helper.Helper.patient));
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(register register)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = register.Email,
                    Email = register.Email,
                    Name = register.Email,
                  // PasswordHash=register.Password

                };
                var result=await  UserManger.CreateAsync(user,register.Password);
                if (result.Succeeded)
                {//assign role to user
                    await UserManger.AddToRoleAsync(user,register.RoleName);
                    if (!User.IsInRole(Helper.Helper.admin))
                    {
                        await SignIn.SignInAsync(user, isPersistent: false);
                    }
                    else
                    {
                        TempData["newAdminSignUp"] = user.Name;
                    }
                    return RedirectToAction("Index", "Appointement");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            return View(register);
        }
        [HttpPost]
        public async Task<IActionResult> Logoff()   
        {
            await SignIn.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
