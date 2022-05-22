using eruditionis.Database;
using eruditionis.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<IActionResult> GetAllMessages(int chatId)
        {
            var result = await _context.Messages
                .Where(m => m.Chat.Id == chatId)
                .Include(m => m.User)
                .Include(m => m.Chat)
                .ToListAsync();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(Message message)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name.ToLower() == message.User.Name.ToLower());
            if (user != null)
            {
                message.User = user;
            }
            else
            {
                return BadRequest("no user with this Name");
            }

            var chat = await _context.Chats.FirstOrDefaultAsync(c => c.Id == message.Chat.Id);
            if (chat != null)
            {
                message.Chat = chat;
            }
            else
            {
                return BadRequest("no chat with this id");
            }

            message.Id = 0;

            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
