using Ecommerce_project.Data;
using Ecommerce_project.DTOs;
using Ecommerce_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_project.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var products = await _context.Products.Include(p => p.ProductCategories)
                                      .ThenInclude(pc => pc.Category)
                                      .Where(p => p.IsActive)
                                      .ToListAsync();

            var productDtos = products.Select(product => ProductToDto(product)).ToList();

            return productDtos;

        }


        // GET: api/Product/1
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id && p.IsActive);

            if (product == null)
            {
                return NotFound();
            }
            return ProductToDto(product);
        }



        // PUT: api/Product/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] int id, ProductDto productDto)
        {
            //if (id != productDto.ProductId)
            //{
            //    return BadRequest();
            //}

            var product = await _context.Products
                .Include(p => p.ProductCategories)
                .FirstOrDefaultAsync(p => p.ProductId == id && p.IsActive);

            if (product == null)
            {
                return NotFound();
            }

            var activeCategoryIds = await _context.Categories
                .Where(c => productDto.CategoryIds.Contains(c.CategoryId) && c.IsActive)
                .Select(c => c.CategoryId)
                .ToListAsync();

            if (!activeCategoryIds.Any())
            {
                return BadRequest("One or more categories are inactive or do not exist.");
            }

            product.ProductName = productDto.ProductName;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.ProductCode = productDto.ProductCode;
            product.CurrentStock = productDto.CurrentStock;
            product.LowStockAlert = productDto.LowStockAlert;
            product.LastUpdatedDate = DateTime.UtcNow;
            product.IsActive = productDto.IsActive;

            // Update categories
            product.ProductCategories.Clear();
            product.ProductCategories = activeCategoryIds.Select(categoryId => new ProductCategory
            {
                ProductId = product.ProductId,
                CategoryId = categoryId
            }).ToList();

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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


        // POST: api/Product
        [HttpPost]
        public async Task<ActionResult<ProductDto>> PostProduct(ProductDto productDto)
        {
            var activeCategoryIds = await _context.Categories
                .Where(c => productDto.CategoryIds.Contains(c.CategoryId) && c.IsActive)
                .Select(c => c.CategoryId)
                .ToListAsync();

            if (!activeCategoryIds.Any())
            {
                return BadRequest("One or more categories are inactive.");
            }

            var product = new Product
            {
                ProductName = productDto.ProductName,
                Description = productDto.Description,
                Price = productDto.Price,
                ProductCode = productDto.ProductCode,
                CurrentStock = productDto.CurrentStock,
                LowStockAlert = productDto.LowStockAlert,
                CreatedDate = DateTime.UtcNow,
                LastUpdatedDate = DateTime.UtcNow,
                IsActive = productDto.IsActive,
                ProductCategories = activeCategoryIds.Select(categoryId => new ProductCategory
                {
                    CategoryId = categoryId
                }).ToList()
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            productDto.ProductId = product.ProductId;

            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, productDto);
        }




        // DELETE: api/Product/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            product.IsActive = false;
            product.LastUpdatedDate = DateTime.UtcNow;

            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Product/search

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> SearchProducts(string keyword)
        {
            var products = await _context.Products
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .Where(p => p.IsActive &&
                            (p.ProductName.Contains(keyword) ||
                             p.Description.Contains(keyword) ||
                             p.ProductCategories.Any(pc => pc.Category.CategoryName.Contains(keyword))))
                .ToListAsync();

            var productDtos = products.Select(product => ProductToDto(product)).ToList();

            return productDtos;
        }

        // GET: api/Product/filter
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> FilterProducts(string? categoryName, decimal? minPrice, decimal? maxPrice)
        {
            IQueryable<Product> productsQuery = _context.Products
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .Where(p => p.IsActive);

            if (!string.IsNullOrEmpty(categoryName))
            {
                int? categoryId = await _context.Categories
                    .Where(c => c.CategoryName == categoryName && c.IsActive)
                    .Select(c => (int?)c.CategoryId)
                    .FirstOrDefaultAsync();

                if (categoryId.HasValue)
                {
                    productsQuery = productsQuery.Where(p => p.ProductCategories.Any(pc => pc.CategoryId == categoryId));
                }
                else
                {
                    return NotFound($"Category '{categoryName}' not found.");
                }
            }

            if (minPrice.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Price >= minPrice);
            }

            if (maxPrice.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Price <= maxPrice);
            }

            var products = await productsQuery.ToListAsync();
            var productDtos = products.Select(product => ProductToDto(product)).ToList();

            return productDtos;
        }

        // GET: api/Product/stocklevels
        [HttpGet("stocklevels")]
        public async Task<ActionResult<IEnumerable<ProductStockDto>>> ViewStockLevels()
        {
            var products = await _context.Products
                .Where(p => p.IsActive)
                .Select(p => new ProductStockDto
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    CurrentStock = p.CurrentStock,
                    LowStockAlert = p.LowStockAlert
                })
                .ToListAsync();

            return products;
        }

        // PUT: api/Product/stocklevels/1
        [HttpPut("stocklevels/{id}")]
        public async Task<IActionResult> UpdateStockLevels([FromRoute] int id, int newStock)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == id && p.IsActive);

            if (product == null)
            {
                return NotFound();
            }

            product.CurrentStock = newStock;
            product.LastUpdatedDate = DateTime.UtcNow;

            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            if (product.CurrentStock <= product.LowStockAlert)
            {
                //Low Stock Alert
            }

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id && e.IsActive);
        }


        private static ProductDto ProductToDto(Product product)
        {
            return new ProductDto
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Description = product.Description,
                Price = product.Price,
                ProductCode = product.ProductCode,
                CurrentStock = product.CurrentStock,
                LowStockAlert = product.LowStockAlert,
                CreatedDate = product.CreatedDate,
                LastUpdatedDate = product.LastUpdatedDate,
                IsActive = product.IsActive,
                CategoryIds = product.ProductCategories.Where(pc => pc.Category.IsActive)
                              .Select(pc => pc.CategoryId)
                              .ToList()
            };
        }

    }

}
