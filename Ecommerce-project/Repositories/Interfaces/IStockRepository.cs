using System;
using Ecommerce_project.DTOs;

namespace Ecommerce_project.Repositories
{
    public interface IStockRepository
    {
        Task<IEnumerable<StockDTO>> GetStockLevels();
        Task UpdateStockLevels(int productId, int newStock);
    }
}

