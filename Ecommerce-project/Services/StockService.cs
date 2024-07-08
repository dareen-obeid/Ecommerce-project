using System;
using Ecommerce_project.DTOs;
using Ecommerce_project.Repositories;
using Ecommerce_project.Services.Interfaces;

namespace Ecommerce_project.Services
{
	public class StockService : IStockService
    {
        private readonly IStockRepository _stockRepository;

        public StockService(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        public async Task<IEnumerable<StockDTO>> GetStockLevels()
        {
            return await _stockRepository.GetStockLevels();
        }

        public async Task UpdateStockLevels(int productId, int newStock)
        {
            await _stockRepository.UpdateStockLevels(productId, newStock);
        }
    }
}

