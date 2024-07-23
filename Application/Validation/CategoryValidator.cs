using System;
using System.ComponentModel.DataAnnotations;
using Ecommerce_project.DTOs;
using Ecommerce_project.Models;

namespace Application.Validation
{
	public class CategoryValidator : IValidator<CategoryDto>
    {

        public void Validate(CategoryDto category)
        {
            if (string.IsNullOrWhiteSpace(category.CategoryName))
                throw new ValidationException("Category name must not be empty.");
        }
    }
}

