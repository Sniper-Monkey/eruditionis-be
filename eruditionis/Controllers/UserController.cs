using eruditionis.Database;
using eruditionis.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

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
        public IActionResult Create(User user)
        {
            user.Id = 0;

            if (_context.Users.FirstOrDefault(u => u.Name.ToLower() == user.Name.ToLower()) != null)
            {
                return BadRequest("user with this name already exists");
            }

            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok();
        }
    }
}
