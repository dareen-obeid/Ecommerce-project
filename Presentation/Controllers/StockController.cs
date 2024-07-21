using Ecommerce_project.DTOs;
using Ecommerce_project.Services;
using Microsoft.AspNetCore.Mvc;


namespace Ecommerce_project.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockController : ControllerBase
    {
        private readonly IProductService _productService;

        public StockController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StockDto>>> GetAllStocks()
        {
            var stocks = await _productService.GetStockLevels();
            return Ok(stocks);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<StockDto>> GetStock(int id)
        {
            var stock = await _productService.GetStockById(id);
            return Ok(stock);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] int newStock)
        {

                await _productService.UpdateStock(id, newStock);
                return NoContent();

        }
    }
}