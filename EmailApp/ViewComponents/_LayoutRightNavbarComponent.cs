using EmailApp.Context;
using EmailApp.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmailApp.ViewComponents
{
    public class _LayoutRightNavbarComponent(AppDbContext _context,UserManager<AppUser> _userManager) :ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.Me = user;

            // Son 5 okunmamış mesajı getir
            var messages = await _context.Messages
                .Include(x => x.Sender)
                .Include(x => x.Receiver)
                .Where(x => x.ReceiverId == user.Id && !x.IsRead)
                .OrderByDescending(x => x.SendDate)
                .Take(5)
                .ToListAsync();
            return View(messages ?? new List<Message>());
        }
    }
}
