using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ResturantWebsite.Models;
using System.Security.Claims;

namespace ResturantWebsite.Controllers
{
    public class AccountController : Controller
    {
        private static readonly List<User> _users = new List<User>
        {
            new User { Username = "admin", Password = "admin123" } 
        };

        public IActionResult Index()

        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model)

        {
            if (ModelState.IsValid)
            {
                var user = _users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);
                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username),

                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                    return RedirectToAction("Index", "Admin");
                }

                ModelState.AddModelError("", "Invalid username or password.");
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }




    }
}
