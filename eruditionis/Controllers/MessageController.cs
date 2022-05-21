using eruditionis.Database;
using eruditionis.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace eruditionis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : Controller
    {
        private readonly ILogger<DocumentController> _logger;
        private readonly DataContext _context;

        public MessageController(ILogger<DocumentController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpGet]
        public Message[] GetAllMessages(int chatId)
        {
            return _context.Messages
                .Where(m => m.Chat.Id == chatId)
                .Include(m => m.User)
                .Include(m => m.Chat)
                .ToArray();
        }

        [HttpPost]
        public IActionResult CreateMessage(Message message)
        {
            var user = _context.Users.FirstOrDefault(u => u.Name.ToLower() == message.User.Name.ToLower());
            if (user != null)
            {
                message.User = user;
            }
            else
            {
                return BadRequest("no user with this Name");
            }

            var chat = _context.Chats.FirstOrDefault(c => c.Id == message.Chat.Id);
            if (chat != null)
            {
                message.Chat = chat;
            }
            else
            {
                return BadRequest("no chat with this id");
            }

            message.Id = 0;

            _context.Messages.Add(message);
            _context.SaveChanges();

            return Ok();
        }
    }
}
