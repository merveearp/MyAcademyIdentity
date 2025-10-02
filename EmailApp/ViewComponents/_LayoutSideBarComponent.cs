using EmailApp.Context;
using EmailApp.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmailApp.ViewComponents
{
    public class _LayoutSideBarComponent(AppDbContext _context, UserManager<AppUser> _userManager) :ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user =await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.Message = _context.Messages.Include(x => x.Sender).Where(x => x.ReceiverId == user.Id).Count();
            ViewBag.UnReadMessage = _context.Messages.Include(x => x.Sender).Where(x => x.ReceiverId == user.Id && x.IsRead == false).Count();
            ViewBag.ReadMessage = _context.Messages.Include(x => x.Sender).Where(x => x.ReceiverId == user.Id && x.IsRead == true).Count();
            ViewBag.TrashMessage = _context.Messages.Include(x => x.Sender).Where(x => x.ReceiverId == user.Id && x.IsDeleted == true).Count();
            ViewBag.DraftMessage = _context.Messages.Include(x => x.Receiver).Where(x => x.SenderId == user.Id && x.IsDraft == true).Count();
            ViewBag.SendedMessage = _context.Messages.Include(x => x.Receiver).Where(x => x.SenderId == user.Id).Count();
            ViewBag.StarMessage = _context.Messages.Include(x => x.Sender).Where(x => x.ReceiverId == user.Id && x.IsStarred == true).Count();
            ViewBag.FlagMessage = _context.Messages.Include(x => x.Sender).Where(x => x.ReceiverId == user.Id && x.IsFlag == true).Count();


            return View();
        }
    }
}
