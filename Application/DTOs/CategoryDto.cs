using System;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce_project.DTOs
{
	public class CategoryDto
	{
        [Required]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Category name must be between 1 and 100 characters.")]
        public string CategoryName { get; set; }

        public bool IsActive { get; set; }
    }
}

