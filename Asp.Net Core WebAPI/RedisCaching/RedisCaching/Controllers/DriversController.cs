using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedisCaching.Data;
using RedisCaching.Models;
using RedisCaching.Services;

namespace RedisCaching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ICacheService _cacheService;

        public DriversController(AppDbContext context, ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
        }

        [HttpGet("Drivers")]
        public async Task<IActionResult> Get()
        {
            // Check cache data
            var cacheData = _cacheService.GetData<IEnumerable<Driver>>("drivers");

            if (cacheData != null && cacheData.Count() > 0)
                return Ok(cacheData);

            cacheData = await _context.Drivers.ToListAsync();

            // Set expiry time and data
            var expiryTime = DateTimeOffset.Now.AddSeconds(30);
            _cacheService.SetData<IEnumerable<Driver>>("drivers", cacheData, expiryTime);
            return Ok(cacheData);
        }

        [HttpPost("AddDriver")]
        public async Task<IActionResult> Post([FromBody] Driver driver)
        {
            var addedObject = await _context.Drivers.AddAsync(driver);
            var expiryTime = DateTimeOffset.Now.AddSeconds(30);

            _cacheService.SetData<Driver>($"driver{driver.Id}", addedObject.Entity, expiryTime);
            await _context.SaveChangesAsync();
            return Ok(addedObject.Entity);
        }

        [HttpDelete("DeleteDriver")]
        public async Task<IActionResult> Delete(int id)
        {
            var exist = await _context.Drivers.FirstOrDefaultAsync(d => d.Id == id);

            if (exist != null)
            {
                _context.Remove(exist);
                _cacheService.RemoveData($"driver{id}");
                await _context.SaveChangesAsync();

                return NoContent();
            }

            return NotFound();

        }

    }
}
