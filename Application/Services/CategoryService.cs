using System;
using System.ComponentModel.DataAnnotations;
using Application.Validation;
using AutoMapper;
using Domain.Exceptions;
using Ecommerce_project.DTOs;
using Ecommerce_project.Models;
using Ecommerce_project.RepositoriyInterfaces;

namespace Ecommerce_project.Services
{

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CategoryDto> _categoryValidator;

        public CategoryService(ICategoryRepository repository, IMapper mapper, IValidator<CategoryDto> categoryValidator)
        {
            _categoryRepository = repository;
            _mapper = mapper;
            _categoryValidator = categoryValidator;
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
                throw new NotFoundException($"Category with ID {id} not found.");
            }
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task UpdateCategory(int id, CategoryDto categoryDto)
        {
            var category = await _categoryRepository.GetCategoryById(id);

            if (category == null || !category.IsActive)
            {
                throw new NotFoundException($"Category with ID {id} not found or is inactive.");
            }

            _categoryValidator.Validate(categoryDto);

            _mapper.Map(categoryDto, category);
                await _categoryRepository.UpdateCategory(category);
        }

        public async Task AddCategory(CategoryDto categoryDto)
        {
            _categoryValidator.Validate(categoryDto);
            var category = _mapper.Map<Category>(categoryDto);
            await _categoryRepository.AddCategory(category);
        }



        public async Task DeleteCategory(int id)
        {
            var category = await _categoryRepository.GetCategoryById(id);
            if (category == null)
            {
                throw new NotFoundException($"Category with ID {id} not found.");
            }
            await _categoryRepository.DeleteCategory(id);
        }

    }
}

