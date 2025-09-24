using EmailApp.Context;
using EmailApp.Entities;
using EmailApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmailApp.Controllers
{
    [Authorize]
    public class MessageController(AppDbContext _context, UserManager<AppUser> _userManager) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var messages = _context.Messages.Include(x => x.Sender).Where(x => x.ReceiverId == user.Id).ToList();

            return View(messages);
        }

        public IActionResult MessageDetail(int id)
        {
            var message = _context.Messages.Include(x => x.Sender).FirstOrDefault(x => x.MessageId == id);

            return View(message);
        }

        [HttpGet]
        public async Task<IActionResult> ReadMessage(bool isRead)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var messages = _context.Messages.Include(x => x.Sender).Where(x => x.ReceiverId == user.Id).ToList();
            ViewBag.IsRead = isRead;
            return View(messages);
        }

        [HttpPost]
        public async Task<IActionResult> ReadMessage(int messageId, bool isRead)
        {
            var message = await _context.Messages.FindAsync(messageId);
            message.IsRead = !message.IsRead;
            await _context.SaveChangesAsync();
            return RedirectToAction("MessageDetail", new { id = messageId });
        }


        [HttpGet]
        public async Task<IActionResult> MoveToTrash()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return Unauthorized();

            var messages = await _context.Messages
                .Include(x => x.Sender)
                .Where(x => x.ReceiverId == user.Id && x.IsDeleted == true)
                .OrderByDescending(m => m.SendDate)
                .ToListAsync();

            return View(messages);
        }

        [HttpPost]
        public async Task<IActionResult> MoveToTrash(int messageId, bool isDeleted)
        {
            var message = await _context.Messages.FindAsync(messageId);
            message.IsDeleted = !message.IsDeleted;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MoveToTrash), new { isDeleted = true });

        }

        [HttpGet]
        public async Task<IActionResult> MoveToDraft()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return Unauthorized();

            var messages = await _context.Messages
                .Include(x => x.Receiver)
                .Where(x => x.SenderId == user.Id && x.IsDraft == true)
                .OrderByDescending(m => m.SendDate)
                .ToListAsync();

            return View(messages);
        }

        [HttpPost]
        public async Task<IActionResult> MoveToDraft(int messageId, bool isDraft)
        {
            var message = await _context.Messages.FindAsync(messageId);
            message.IsDraft = !message.IsDraft;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MoveToDraft), new { isDraft = true });


        }

        [HttpGet]
        public async Task<IActionResult> SendedMessages()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var messages = await _context.Messages
                .Include(x => x.Receiver)
                .Where(x => x.SenderId == user.Id)
                .OrderByDescending(m => m.SendDate) // varsa tarih alanı
                .ToListAsync();

            return View(messages);
        }

        public IActionResult SendMessage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(SendMessageViewModel model)
        {
            var sender = await _userManager.FindByNameAsync(User.Identity.Name);
            var receiver = await _userManager.FindByEmailAsync(model.ReceiverEmail);


            var message = new Message()
            {
                Body = model.Body,
                Subject = model.Subject,
                ReceiverId = receiver.Id,
                SenderId = sender.Id,
                SendDate = DateTime.Now,
            };
            _context.Messages.Add(message);
            _context.SaveChanges();
            return RedirectToAction("Index");


        }
        public IActionResult SendedMessageDetail(int id)
        {
            var message = _context.Messages.Include(x => x.Receiver).FirstOrDefault(x => x.MessageId == id);

            return View(message);
        }

        [HttpGet]
        public async Task<IActionResult> Starred()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var list = await _context.Messages
                .Include(x => x.Sender)
                .Where(x => x.ReceiverId == user.Id && x.IsStarred ==true )
                .OrderByDescending(x => x.SendDate)
                .ToListAsync();

            return View(list);
        }

        [HttpPost]     
        public async Task<IActionResult> ToggleStar(int id, string? redirect = null)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var msg = await _context.Messages
                .FirstOrDefaultAsync(m => m.MessageId == id && m.ReceiverId == user.Id);
            
            msg.IsStarred = !msg.IsStarred;
            await _context.SaveChangesAsync();

            return !string.IsNullOrEmpty(redirect)
                ? Redirect(redirect)
                : RedirectToAction(nameof(Starred));
        }

        [HttpGet]
        public async Task<IActionResult> IsFlag()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var list = await _context.Messages
                .Include(x => x.Sender)
                .Where(x => x.ReceiverId == user.Id && x.IsFlag ==true)
                .OrderByDescending(x => x.SendDate)
                .ToListAsync();

            return View(list);
        }

        [HttpPost]
      
        public async Task<IActionResult> ToggleFlag(int id, string? redirect = null)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var msg = await _context.Messages
                .FirstOrDefaultAsync(m => m.MessageId == id && m.ReceiverId == user.Id);
           
            msg.IsFlag = !msg.IsFlag;
            await _context.SaveChangesAsync();

            return !string.IsNullOrEmpty(redirect)
                ? Redirect(redirect)
                : RedirectToAction(nameof(IsFlag));
        }


    }

    }
