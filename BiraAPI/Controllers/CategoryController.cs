using BiraAPI.Data;
using BiraAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BiraAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly DataContext _context;
        public CategoryController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Category>>> Get()
        {
            return Ok(await _context.Categories.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> Get(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return BadRequest("Category not found.");
            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<List<Category>>> AddCategory(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return Ok(await _context.Categories.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<List<Category>>> UpdateCategory(Category request)
        {
            var dbCategory = await _context.Categories.FindAsync(request.Id);
            if (dbCategory == null)
                return BadRequest("Category not found.");

            dbCategory.Name = request.Name;

            await _context.SaveChangesAsync();

            return Ok(await _context.Categories.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Category>>> Delete(int id)
        {
            var dbCategory = await _context.Categories.FindAsync(id);
            if (dbCategory == null)
                return BadRequest("Category not found.");

            _context.Categories.Remove(dbCategory);
            await _context.SaveChangesAsync();

            return Ok(await _context.Categories.ToListAsync());
        }
    }
}
