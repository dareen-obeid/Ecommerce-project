using AutoMapper;
using Ecommerce_project.Data;
using Ecommerce_project.DTOs;
using Ecommerce_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_project.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;


        public CategoryController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        // GET: api/Category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {
            var categories = await _context.Categories
                .Where(c => c.IsActive)
                .ToListAsync();

            return _mapper.Map<List<CategoryDto>>(categories);
        }


        // GET: api/Category/1
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(int id)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryId == id && c.IsActive);

            if (category == null)
            {
                return NotFound();
            }

            return _mapper.Map<CategoryDto>(category);
        }


        // PUT: api/Category/1
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory([FromRoute] int id, [FromBody] CategoryDto categoryDto)
        {

            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryId == id && c.IsActive);

            if (category == null)
            {
                return NotFound();
            }

            _mapper.Map(categoryDto, category);
            category.CategoryId = id;

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Category
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> PostCategory(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            categoryDto.CategoryId = category.CategoryId;

            return CreatedAtAction(nameof(GetCategory), new { id = category.CategoryId }, categoryDto);
        }


        // DELETE: api/Category/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var productCategories = await _context.ProductCategories
                .Where(pc => pc.CategoryId == id)
                .ToListAsync();

            _context.ProductCategories.RemoveRange(productCategories);


            category.IsActive = false;
            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id && e.IsActive);
        }


       


    }
}

