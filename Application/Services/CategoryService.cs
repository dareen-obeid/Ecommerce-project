using System;
using AutoMapper;
using Ecommerce_project.DTOs;
using Ecommerce_project.Models;
using Ecommerce_project.RepositoriyInterfaces;

namespace Ecommerce_project.Services
{

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository repository, IMapper mapper)
        {
            _categoryRepository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllActiveCategories()
        {
            var categories = await _categoryRepository.GetAllActiveCategories();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto> GetCategoryById(int id)
        {
            var category = await _categoryRepository.GetCategoryById(id);
            if (category == null)
            {
                return null;
            }
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task UpdateCategory(int id, CategoryDto categoryDto)
        {
            var category = await _categoryRepository.GetCategoryById(id);
            if (category != null || category.IsActive)
            {
                _mapper.Map(categoryDto, category);
                await _categoryRepository.UpdateCategory(category);
            }
        }

        public async Task AddCategory(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            await _categoryRepository.AddCategory(category);
        }



        public async Task DeleteCategory(int id)
        {
            await _categoryRepository.DeleteCategory(id);
        }

    }
}

