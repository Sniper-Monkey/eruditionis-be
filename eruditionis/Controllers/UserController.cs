using eruditionis.Database;
using eruditionis.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace eruditionis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly DataContext _context;

        public UserController(ILogger<UserController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            user.Id = 0;
            var userFromDb = _context.Users.FirstOrDefault(u => u.Name.ToLower() == user.Name.ToLower());
            if (userFromDb != null)
            {
                return BadRequest("user with this name already exists");
            }

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
