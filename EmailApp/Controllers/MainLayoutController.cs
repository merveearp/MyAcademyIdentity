using Microsoft.AspNetCore.Mvc;

namespace EmailApp.Controllers
{
    public class MainLayoutController : Controller
    {
        public IActionResult Layout()
        {
            return View();
        }
    }
}
