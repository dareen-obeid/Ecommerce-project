using System;
using System.ComponentModel.DataAnnotations;
using Domain.Exceptions;
using Ecommerce_project.Data;
using Ecommerce_project.Models;
using Ecommerce_project.RepositoriyInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_project.Repositories
{
	public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllActiveProducts()
        {
            return await _context.Products
                              .Include(p => p.ProductCategories)
                              .ThenInclude(pc => pc.Category)
                              .Where(p => p.IsActive)
                              .ToListAsync();
        }


        public async Task<Product> GetProductById(int id)
        {
            var Product = await _context.Products
                              .Include(p => p.ProductCategories)
                              .ThenInclude(pc => pc.Category)
                              .FirstOrDefaultAsync(p => p.ProductId == id && p.IsActive);

            if (Product == null)
            {
                throw new NotFoundException($"Category with ID {id} not found.");
            }

            return Product;

        }



        public async Task UpdateProduct(Product product, IEnumerable<int> newCategoryIds)
        {
            var existingProduct = await GetProductById(product.ProductId);
            if (existingProduct == null)
            {
                throw new NotFoundException("Product not found.");
            }

            var categories = await _context.Categories
                .Where(c => newCategoryIds.Contains(c.CategoryId) && c.IsActive)
                .ToListAsync();

            if (!categories.Any())
            {
                throw new ValidationException("One or more categories are inactive or do not exist.");
            }

            product.LastUpdatedDate = DateTime.UtcNow;

            existingProduct.ProductCategories.Clear();
            existingProduct.ProductCategories = categories.Select(c => new ProductCategory { CategoryId = c.CategoryId, ProductId = existingProduct.ProductId }).ToList();

            _context.Entry(existingProduct).CurrentValues.SetValues(product);
            await _context.SaveChangesAsync();
        }


        public async Task AddProduct(Product product, IEnumerable<int> categoryIds)
        {
            var categories = await _context.Categories
                .Where(c => categoryIds.Contains(c.CategoryId) && c.IsActive)
                .ToListAsync();

            if (!categories.Any())
            {
                throw new ValidationException("One or more categories are inactive or do not exist.");
            }

            product.ProductCategories = categories.Select(c => new ProductCategory { CategoryId = c.CategoryId }).ToList();
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.ProductCategories) 
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product != null)
            {
                _context.ProductCategories.RemoveRange(product.ProductCategories);

                product.IsActive = false;
                product.LastUpdatedDate = DateTime.UtcNow;
                _context.Entry(product).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ProductExists(int id)
        {
            return await _context.Products.AnyAsync(p => p.ProductId == id && p.IsActive);
        }

        public async Task<IEnumerable<Product>> SearchProducts(string keyword)
        {
            return await _context.Products
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .Where(p => p.IsActive &&
                            (p.ProductName.Contains(keyword) ||
                             p.Description.Contains(keyword) ||
                             p.ProductCategories.Any(pc => pc.Category.CategoryName.Contains(keyword))))
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> FilterProducts(string category, decimal? minPrice, decimal? maxPrice)
        {
            IQueryable<Product> productsQuery = _context.Products
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .Where(p => p.IsActive);

            if (!string.IsNullOrEmpty(category))
            {
                productsQuery = productsQuery.Where(p => p.ProductCategories.Any(pc => pc.Category.CategoryName == category));
            }

            if (minPrice.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Price <= maxPrice.Value);
            }

            return await productsQuery.ToListAsync();
        }


        public async Task<IEnumerable<Product>> GetStockLevels()
        {
            return await _context.Products
                                 .Where(p => p.IsActive)
                                 .Select(p => new Product
                                 {
                                     ProductId = p.ProductId,
                                     ProductName = p.ProductName,
                                     CurrentStock = p.CurrentStock,
                                     LowStockAlert = p.LowStockAlert
                                 })
                                 .ToListAsync();
        }

        public async Task UpdateStock(int id, int newStock)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null && product.IsActive)
            {
                product.CurrentStock = newStock;
                product.LastUpdatedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsProductCodeUnique(string productCode, int productId)
        {
            return !await _context.Products.AnyAsync(p => p.ProductCode == productCode && p.ProductId != productId);
        }
    }
}
