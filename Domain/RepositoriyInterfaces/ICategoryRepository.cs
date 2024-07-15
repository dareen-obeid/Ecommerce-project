using System;
using Ecommerce_project.Models;

namespace Ecommerce_project.RepositoriyInterfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllActiveCategories();
        Task<Category> GetCategoryById(int id);
        Task AddCategory(Category category);
        Task UpdateCategory(Category category);
        Task DeleteCategory(int id);
        Task<bool> CategoryExists(int id);

    }

}

