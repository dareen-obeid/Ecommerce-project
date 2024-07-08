using Ecommerce_project.DTOs;
using Ecommerce_project.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace Ecommerce_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;

        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }

        // GET: api/stock/stocklevels
        [HttpGet("stocklevels")]
        public async Task<ActionResult<IEnumerable<StockDTO>>> ViewStockLevels()
        {
            return Ok(await _stockService.GetStockLevels());
        }

        // PUT: api/stock/stocklevels/{id}
        [HttpPut("stocklevels/{id}")]
        public async Task<IActionResult> UpdateStockLevels(int id, int newStock)
        {

                await _stockService.UpdateStockLevels(id, newStock);
                return NoContent();
           
        }
    }
}

