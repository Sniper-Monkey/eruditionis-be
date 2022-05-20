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
    public class ChatController : ControllerBase
    {
        private readonly ILogger<DocumentController> _logger;
        private readonly DataContext _context;

        public ChatController(ILogger<DocumentController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public Chat[] GetAllChats()
        {
            return _context.Chats
                .ToArray();
        }


        [HttpPost]
        public IActionResult CreateChat(Chat chat)
        {
            _context.Chats.Add(chat);
            _context.SaveChanges();

            return Ok();
        }

    }
}
