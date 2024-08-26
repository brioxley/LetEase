using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LetEase.Infrastructure.Data;
using LetEase.Domain.Entities;

namespace LetEase.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PropertiesController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public PropertiesController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: api/Properties
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Property>>> GetProperties()
		{
			return await _context.Properties.ToListAsync();
		}

		// GET: api/Properties/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Property>> GetProperty(int id)
		{
			var property = await _context.Properties.FindAsync(id);

			if (property == null)
			{
				return NotFound();
			}

			return property;
		}

		// POST: api/Properties
		[HttpPost]
		public async Task<ActionResult<Property>> PostProperty(Property property)
		{
			_context.Properties.Add(property);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetProperty", new { id = property.Id }, property);
		}
	}
}


