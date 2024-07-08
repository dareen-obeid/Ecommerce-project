using System;
using Ecommerce_project.DTOs;

namespace Ecommerce_project.Services.Interfaces
{
    public interface IStockService
    {
        Task<IEnumerable<StockDTO>> GetStockLevels();
        Task UpdateStockLevels(int productId, int newStock);
    }
}

