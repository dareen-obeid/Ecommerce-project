using System;
using AutoMapper;
using Ecommerce_project.DTOs;
using Ecommerce_project.Models;
using Ecommerce_project.Repositories;

namespace Ecommerce_project.Services
{
	public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }


        public async Task<IEnumerable<ProductDto>> GetAllActiveProducts()
        {
            var products = await _productRepository.GetAllActiveProducts();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto> GetProductById(int id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null)
            {
                return null;
            }
            return _mapper.Map<ProductDto>(product);
        }

        public async Task UpdateProduct(int id, ProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            await _productRepository.UpdateProduct(product, productDto.CategoryIds);
        }

        public async Task AddProduct(ProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            await _productRepository.AddProduct(product, productDto.CategoryIds);
        }

        public async Task DeleteProduct(int id)
        {
            await _productRepository.DeleteProduct(id);
        }


        public async Task<IEnumerable<ProductDto>> SearchProducts(string keyword)
        {
            var products = await _productRepository.SearchProducts(keyword);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<IEnumerable<ProductDto>> FilterProducts(string category, decimal? minPrice, decimal? maxPrice)
        {
            var products = await _productRepository.FilterProducts(category, minPrice, maxPrice);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<IEnumerable<StockDto>> GetStockLevels()
        {
            var products = await _productRepository.GetAllActiveProducts();
            return _mapper.Map<IEnumerable<StockDto>>(products);
        }

        public async Task<StockDto> GetStockById(int id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null)
            {
                return null;
            }
            return _mapper.Map<StockDto>(product);
        }

        public async Task UpdateStock(int id, int newStock)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null || !product.IsActive)
            {
                throw new KeyNotFoundException($"Product with ID {id} not found or inactive.");
            }

            if (newStock <= product.LowStockAlert)
            {
                Console.WriteLine($"Warning: Stock for Product ID {id} is low.");
            }

            await _productRepository.UpdateStock(id, newStock);
        }
    }
}

