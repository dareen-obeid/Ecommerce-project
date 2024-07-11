using System;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce_project.Models
{
	public class Product
	{

        [Key]
        public int ProductId { get; set; }

        [Required]
        [StringLength(100)]
        public string ProductName { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Range(0, 10000)]
        public decimal Price { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductCode { get; set; }

        [Range(0, 10000)]
        public int CurrentStock { get; set; }

        [Range(0, 10000)]
        public int LowStockAlert { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime LastUpdatedDate { get; set; }

        [Required]
        public bool IsActive { get; set; }


        public ICollection<ProductCategory> ProductCategories { get; set; }



    }

}

