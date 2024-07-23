using System;
using Ecommerce_project.Models;

namespace Ecommerce_project.RepositoriyInterfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllActiveProducts();
        Task<Product> GetProductById(int id);
        Task AddProduct(Product product, IEnumerable<int> categoryIds);
        Task UpdateProduct(Product product, IEnumerable<int> newCategoryIds);
        Task DeleteProduct(int id);
        Task<bool> ProductExists(int id);

        Task<IEnumerable<Product>> SearchProducts(string keyword);
        Task<IEnumerable<Product>> FilterProducts(string category, decimal? minPrice, decimal? maxPrice);

        Task UpdateStock(int id, int newStock);
        Task<bool> IsProductCodeUnique(string productCode, int productId);
    }

}


