using System;
using Domain.Exceptions;
using Ecommerce_project.Data;
using Ecommerce_project.Models;
using Ecommerce_project.RepositoriyInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_project.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllActiveCategories()
        {
            return await _context.Categories.Where(c => c.IsActive).ToListAsync();
        }

        public async Task<Category> GetCategoryById(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id && c.IsActive);

            if (category == null)
            {
                throw new NotFoundException($"Category with ID {id} not found.");
            }

            return category;
        }


        public async Task UpdateCategory(Category category)
        {
            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task AddCategory(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                throw new NotFoundException("Category not found.");
            }

            var productCategories = await _context.ProductCategories
                    .Where(pc => pc.CategoryId == id)
                    .ToListAsync();
                _context.ProductCategories.RemoveRange(productCategories);

                category.IsActive = false;
                _context.Entry(category).State = EntityState.Modified;

                await _context.SaveChangesAsync();
         
        }

        
        public async Task<bool> CategoryExists(int id)
        {
            return await _context.Categories.AnyAsync(e => e.CategoryId == id && e.IsActive);
        }
    }
}