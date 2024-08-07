﻿using Ecommerce_project.DTOs;
using Ecommerce_project.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_project.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/Category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {
            var categories = await _categoryService.GetAllActiveCategories();
            return Ok(categories);
        }

        // GET: api/Category/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(int id)
        {
            var category = await _categoryService.GetCategoryById(id);
            return Ok(category);
        }

        // PUT: api/Category/1
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutCategory([FromRoute] int id, CategoryDto categoryDto)
        {
            await _categoryService.UpdateCategory(id, categoryDto);

            return NoContent();
        }

        // POST: api/Category
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> PostCategory(CategoryDto categoryDto)
        {
            await _categoryService.AddCategory(categoryDto);
            return CreatedAtAction(nameof(GetCategory), new { id = categoryDto.CategoryId }, categoryDto);
        }

        // DELETE: api/Category/1
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryService.DeleteCategory(id);
            return NoContent();
        }
    }
}

