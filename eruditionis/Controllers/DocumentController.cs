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
        public IEnumerable<Document> ReadAll()
        {
            return _context.Documents
                .Include(d => d.UploadedBy)
                .ToArray();
        }

        [HttpPost]
        public IActionResult Create(Document document)
        {
            document.Id = 0;
            _context.Add<Document>(document);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPut]
        public IActionResult Delete(int id)
        {
            var doc = _context.Documents.First(d => d.Id == id);
            _context.Documents.Remove(doc);
            _context.SaveChanges();

            return Ok();
        }
    }
}
