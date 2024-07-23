using System;
using System.ComponentModel.DataAnnotations;
using Ecommerce_project.DTOs;

namespace Application.Validation
{
    public class ProductValidator : IValidator<ProductDto>
    {

        public void Validate(ProductDto product)
        {
            if (product.Price <= 0 || product.Price > 10000.00m)
                throw new ValidationException("Price must be between 0.01 and 10000.00.");

            if (string.IsNullOrWhiteSpace(product.ProductName) || product.ProductName.Length > 100)
                throw new ValidationException("Product name must be between 1 and 100 characters.");

            if (string.IsNullOrWhiteSpace(product.ProductCode) || product.ProductCode.Length > 50)
                throw new ValidationException("Product code must be up to 50 characters.");

            if (product.CurrentStock < 0)
                throw new ValidationException("Current stock must be a non-negative number.");

            if (product.LowStockAlert < 0)
                throw new ValidationException("Low stock alert level must be a non-negative number.");

            if (!string.IsNullOrWhiteSpace(product.Description) && product.Description.Length > 1000)
                throw new ValidationException("The description must be less than 1000 characters long.");

            if (product.CreatedDate == default)
                throw new ValidationException("Creation date is required.");

            if (product.LastUpdatedDate == default)
                throw new ValidationException("Last updated date is required.");


            if (string.IsNullOrWhiteSpace(product.ProductCode))
            {
                throw new ValidationException("Product code is required and must be unique.");
            }

        }
    }
}
