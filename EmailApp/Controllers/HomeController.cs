using System.Diagnostics;
using EmailApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace EmailApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
       
        public IActionResult Index()
        {
            return View();
        }

    }
}
