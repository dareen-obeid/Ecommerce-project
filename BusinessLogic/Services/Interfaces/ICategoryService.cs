using System;
using Ecommerce_project.DTOs;

namespace Ecommerce_project.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllActiveCategories();
        Task<CategoryDto> GetCategoryById(int id);
        Task AddCategory(CategoryDto categoryDto);
        Task UpdateCategory(int id, CategoryDto categoryDto);
        Task DeleteCategory(int id);
    }

}

