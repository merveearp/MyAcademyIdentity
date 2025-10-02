using EmailApp.Entities;
using EmailApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EmailApp.Controllers
{
    public class RegisterController(UserManager<AppUser> _userManager) : Controller
    {
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(RegisterViewModel model)
        {
            var user=new AppUser
            {
                Email=model.Email,
                FirstName=model.FirstName,
                LastName=model.LastName,
                UserName=model.UserName,
                ImageUrl = "~/AdminLTE-3.0.4/dist/img/defaultuser.png"
            };
            var result =await _userManager.CreateAsync(user,model.Password);
            if(!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);                   
                }
                return View(model);
            }
            return RedirectToAction("Index", "Login");
            
        }
    }
}
