using System;
using System.ComponentModel.DataAnnotations;
using Application.Validation;
using AutoMapper;
using Domain.Exceptions;
using Ecommerce_project.DTOs;
using Ecommerce_project.Models;
using Ecommerce_project.RepositoriyInterfaces;

namespace Ecommerce_project.Services
{
	public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<ProductDto> _productValidator;


        public ProductService(IProductRepository productRepository, IMapper mapper, IValidator<ProductDto> productValidator)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _productValidator = productValidator;

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
                throw new NotFoundException($"Product with ID {id} not found.");
            }
            return _mapper.Map<ProductDto>(product);
        }

        public async Task UpdateProduct(int id, ProductDto productDto)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null)
            {
                throw new NotFoundException($"Product with ID {id} not found.");
            }
            _productValidator.Validate(productDto);
            if (!await _productRepository.IsProductCodeUnique(productDto.ProductCode, productDto.ProductId))
            {
                throw new ValidationException("Product code must be unique.");
            }
            _mapper.Map(productDto, product);
            await _productRepository.UpdateProduct(product, productDto.CategoryIds);

        }

        public async Task AddProduct(ProductDto productDto)
        {
            _productValidator.Validate(productDto);

            if (!await _productRepository.IsProductCodeUnique(productDto.ProductCode, productDto.ProductId))
            {
                throw new ValidationException("Product code must be unique.");
            }
            var product = _mapper.Map<Product>(productDto);
            await _productRepository.AddProduct(product, productDto.CategoryIds);
        }

        public async Task DeleteProduct(int id)
        {
            var product = await _productRepository.GetProductById(id);

            if (product == null)
            {
                throw new NotFoundException($"Product with ID {id} not found.");
            }
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
                throw new NotFoundException($"Product with ID {id} not found.");
            }
            return _mapper.Map<StockDto>(product);
        }

        public async Task UpdateStock(int id, int newStock)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null || !product.IsActive)
            {
                throw new NotFoundException($"Product with ID {id} not found or inactive.");
            }
            if (newStock < 0)
            {
                throw new ValidationException("Stock level cannot be negative.");
            }

            if (newStock <= product.LowStockAlert)
            {
                Console.WriteLine($"Warning: Stock for Product ID {id} is low.");
            }

            await _productRepository.UpdateStock(id, newStock);
        }
    }
}

