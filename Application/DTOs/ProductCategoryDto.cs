using System;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce_project.DTOs
{
	public class ProductCategoryDto
	{
        [Required]
        public int ProductCategoryId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}

