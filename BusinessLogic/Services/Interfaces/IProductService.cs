using System;
using Ecommerce_project.DTOs;

namespace Ecommerce_project.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllActiveProducts();
        Task<ProductDto> GetProductById(int id);
        Task AddProduct(ProductDto productDto);
        Task UpdateProduct(int id, ProductDto productDto);
        Task DeleteProduct(int id);
        Task<IEnumerable<ProductDto>> SearchProducts(string keyword);
        Task<IEnumerable<ProductDto>> FilterProducts(string category, decimal? minPrice, decimal? maxPrice);

        Task<IEnumerable<StockDto>> GetStockLevels();
        Task UpdateStock(int id, int newStock);
    }
}

