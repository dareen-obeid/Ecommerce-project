using System;
namespace Ecommerce_project.DTOs
{
	public class StockDTO
	{
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int CurrentStock { get; set; }
        public int LowStockAlert { get; set; }
    }
}

