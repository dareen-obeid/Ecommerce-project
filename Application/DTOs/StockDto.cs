using System;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce_project.DTOs
{
	public class StockDto
	{
        [Required]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        public string ProductName { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Current stock must be a non-negative number.")]
        public int CurrentStock { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Low stock alert level must be a non-negative number.")]
        public int LowStockAlert { get; set; }
    }
}

