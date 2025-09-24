using EmailApp.Entities;
using EmailApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmailApp.Controllers
{
    public class LoginController(UserManager<AppUser> _userManager ,SignInManager<AppUser> _signInManager): Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            var user =await _userManager.FindByEmailAsync(model.Email);
            
            if(user is null)
            {
                ModelState.AddModelError("","Bu Email sistemde kayıtlı değil!");
                return View(model);
            }
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            
            if (!result.Succeeded)
            {
                ModelState.AddModelError("","Email veya Şifre hatalı!");
                return View(model);

            }
            return RedirectToAction("Index","Message");
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Login");
        }
      
    }
}
