using eruditionis.Database;
using eruditionis.Database.Models;
using Microsoft.AspNetCore.Mvc;
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
                .ToArray();
        }

        [HttpPost]
        public IActionResult CreateMessage(Message message)
        {
            _context.Messages.Add(message);
            _context.SaveChanges();

            return Ok();
        }
    }
}
