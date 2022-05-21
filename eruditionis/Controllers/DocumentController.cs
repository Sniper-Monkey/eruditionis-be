using eruditionis.Database;
using eruditionis.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace eruditionis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly ILogger<DocumentController> _logger;
        private readonly DataContext _context;

        public DocumentController(ILogger<DocumentController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        [Route("/[action]")]
        public IEnumerable<Document> ReadAll()
        {
            return _context.Documents
                .Include(d => d.UploadedBy)
                .Include(d => d.Chat)
                .ToArray();
        }

        [HttpGet]
        public Document Read(int docId)
        {
            return _context.Documents
                .Include(d => d.UploadedBy)
                .Include(d => d.Chat)
                .First(d => d.Id == docId);
        }

        [HttpGet]
        [Route("/[action]")]
        public object ReadTitle(int docId)
        {
            return new { Title =
                _context.Documents
                    .Include(d => d.UploadedBy)
                    .Include(d => d.Chat)
                    .First(d => d.Id == docId)
                    .Title };
        }

        [HttpPost]
        public IActionResult Create(Document document)
        {
            var user = _context.Users.FirstOrDefault(u => u.Name.ToLower() == document.UploadedBy.Name.ToLower());
            if (user != null)
            {
                document.UploadedBy = user;
            }
            else
            {
                return BadRequest("no user with this name");
            }

            document.Chat = null;
            document.Id = 0;

            _context.Add<Document>(document);
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var doc = _context.Documents.First(d => d.Id == id);
            _context.Documents.Remove(doc);
            _context.SaveChanges();

            return Ok();
        }
    }
}
