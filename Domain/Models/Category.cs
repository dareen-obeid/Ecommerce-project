using System;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce_project.Models
{
	public class Category
	{
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public ICollection<ProductCategory> ProductCategories { get; set; }

    }
}

