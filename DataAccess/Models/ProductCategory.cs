using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce_project.Models
{
	public class ProductCategory
	{

        [Key]
        public int ProductCategoryId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public Product Product { get; set; }
        public Category Category { get; set; }

    }
}

