using AutoMapper;
using Ecommerce_project.DTOs;
using Ecommerce_project.Models;
using Ecommerce_project.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_project.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;


        public ProductController(IProductService productService)
        {
            _productService = productService;
        }


        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var products = await _productService.GetAllActiveProducts();
            return Ok(products);

        }


        // GET: api/Product/1
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }



        // PUT: api/Product/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] int id, ProductDto productDto)
        {

            if (id != productDto.ProductId)
            {
                return BadRequest();
            }

            await _productService.UpdateProduct(id, productDto);
            return NoContent();
        }


        // POST: api/Product
        [HttpPost]
        public async Task<ActionResult<ProductDto>> PostProduct(ProductDto productDto)
        {
            await _productService.AddProduct(productDto);
            return CreatedAtAction(nameof(GetProduct), new { id = productDto.ProductId }, productDto);
        }


        // DELETE: api/Product/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProduct(id);
            return NoContent();
        }

        // GET: api/Product/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> SearchProducts(string keyword)
        {
            var products = await _productService.SearchProducts(keyword);
            return Ok(products);
        }

        // GET: api/Product/filter
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> FilterProducts(string? categoryName, decimal? minPrice, decimal? maxPrice)
        {
            var products = await _productService.FilterProducts(categoryName, minPrice, maxPrice);
            return Ok(products);
        }



    }

}
