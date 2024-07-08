using System;
using Ecommerce_project.Data;
using Ecommerce_project.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_project.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDbContext _context;

        public StockRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StockDTO>> GetStockLevels()
        {
            return await _context.Products
                .Where(p => p.IsActive)
                .Select(p => new StockDTO
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    CurrentStock = p.CurrentStock,
                    LowStockAlert = p.LowStockAlert
                })
                .ToListAsync();
        }

        public async Task UpdateStockLevels(int productId, int newStock)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == productId && p.IsActive);

            product.CurrentStock = newStock;
            product.LastUpdatedDate = DateTime.UtcNow;


            if (product.CurrentStock <= product.LowStockAlert)
            {
                //todo//Low Stock Alert
            }

            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }

}

