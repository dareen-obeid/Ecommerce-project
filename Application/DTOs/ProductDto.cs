using System;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce_project.DTOs
{
	public class ProductDto
	{
        [Required]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, ErrorMessage = "Product name must be between 1 and 100 characters.")]
        public string ProductName { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Range(0.01, 10000.00, ErrorMessage = "Price must be between 0.01 and 10000.00.")]
        public decimal Price { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Product code must be up to 50 characters.")]
        public string ProductCode { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Low stock alert level must be a non-negative number.")]
        public int CurrentStock { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Low stock alert level must be a non-negative number.")]
        public int LowStockAlert { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime LastUpdatedDate { get; set; }

        public bool IsActive { get; set; }

        public List<int> CategoryIds { get; set; }
    }
}

