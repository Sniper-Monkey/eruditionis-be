using eruditionis.Database;
using eruditionis.Database.Models;
using eruditionis.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eruditionis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly ILogger<ChatController> _logger;
        private readonly DataContext _context;

        public ChatController(ILogger<ChatController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllChats()
        {
            var chats = _context.Chats.ToList();
            List<ChatDto> chatsDto = new List<ChatDto>();

            foreach (var chat in chats)
            {
                var doc = await _context.Documents.FirstOrDefaultAsync(d => d.Chat.Id == chat.Id);

                chatsDto.Add(new ChatDto()
                {
                    Id = chat.Id,
                    Title = chat.Title,
                    DocId = doc.Id
            });

            }

            return Ok(chatsDto);
        }


        [HttpPost]
        public async Task<IActionResult> CreateChat(ChatDto chat)
        {
            Chat chatModel = new Chat(){Title = chat.Title};

            var doc = _context.Documents.FirstOrDefault(d => d.Id == chat.DocId);

            if (doc == null)
            {
                return BadRequest("document with this id doesn't exist");

            }

            _context.Chats.Add(chatModel);
            doc.Chat = chatModel;
            await _context.Chats.AddAsync(chatModel);
            _context.Update(doc);
            await _context.SaveChangesAsync();

            return Ok(chatModel);
        }

    }
}
