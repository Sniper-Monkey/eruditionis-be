using eruditionis.Database;
using eruditionis.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace eruditionis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly DataContext _context;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("GetTest")]
        public async Task<IActionResult> GetTest()
        {
            if(_context.TestingEntities.Count() > 0)
            {
                _context.TestingEntities.RemoveRange(_context.TestingEntities);
                await _context.SaveChangesAsync();
            }

            List<TestingEntity> testingEntities = new List<TestingEntity>();
            for(int i = 1; i <= 5; i++)
            {
                testingEntities.Add(new TestingEntity {
                    Name = "Name " + i.ToString(),
                    Description = "Description " + i.ToString(),
                });
            }

            var testEntity = new TestingEntity
            {
                Name = "NewName",
                Description = "NewDesc",
            };

            await _context.AddRangeAsync(testingEntities);
            await _context.AddAsync(testEntity);
            await _context.SaveChangesAsync();
            var response = _context.TestingEntities.ToList();

            return Ok(response);
        }
    }
}
