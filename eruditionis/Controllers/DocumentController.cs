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
    public class DocumentController : ControllerBase
    {
        private readonly ILogger<DocumentController> _logger;
        private readonly DataContext _context;
        private readonly FileSystemService _fileSystemService;

        public DocumentController(ILogger<DocumentController> logger, DataContext context, FileSystemService fileSystemService)
        {
            _logger = logger;
            _context = context;
            _fileSystemService = fileSystemService;
        }

        [HttpGet]
        [Route("/[action]")]
        public async Task<IActionResult> ReadAll()
        {
            var result = await _context.Documents
                .Include(d => d.UploadedBy)
                .Include(d => d.Chat)
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Read(int docId)
        {
            var result = await _context.Documents
                .Include(d => d.UploadedBy)
                .Include(d => d.Chat)
                .FirstOrDefaultAsync(d => d.Id == docId);

            if(result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("/[action]")]
        public async Task<IActionResult> ReadTitle(int docId)
        {
            var result = await _context.Documents
                    .Include(d => d.UploadedBy)
                    .Include(d => d.Chat)
                    .FirstOrDefaultAsync(d => d.Id == docId);

            if(result == null)
            {
                return NotFound();
            }

            return Ok(result.Title);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PostDocumentViewModel documentVM)
        {
            var user = _context.Users.FirstOrDefault(u => u.Name.ToLower() == documentVM.UserName.ToLower());
            Document document = new Document
            {
                Title = documentVM.Title,
                Description = documentVM.Description
            };
            if (user != null)
            {
                document.UploadedBy = user;
            }
            else
            {
                return BadRequest("no user with this name");
            }

            document.Chat = null;

            _context.Add<Document>(document);
            _context.SaveChanges();

            var route = await _fileSystemService.Create(documentVM.File, document.Id);

            document.File = route;
            _context.Update(document);
            await _context.SaveChangesAsync();

            return Ok(document);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var doc = await _context.Documents.FirstOrDefaultAsync(d => d.Id == id);
            if(doc == null)
            {
                return NotFound();
            }

            _context.Documents.Remove(doc);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
