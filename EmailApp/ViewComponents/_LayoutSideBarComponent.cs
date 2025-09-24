using EmailApp.Context;
using Microsoft.AspNetCore.Mvc;

namespace EmailApp.ViewComponents
{
    public class _LayoutSideBarComponent(AppDbContext _context) :ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            ViewBag.UnReadMessage = _context.Messages.Where(x=>x.IsRead==false).Count();
            return View();
        }
    }
}
