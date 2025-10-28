using E_Commerce_Shop_Api.Data;
using E_Commerce_Shop_Api.Data.Models;
using E_Commerce_Shop_Api.Dtos.Requests;
using E_Commerce_Shop_Api.Dtos.Responses;
using E_Commerce_Shop_Api.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace E_Commerce_Shop_Api.Services.Implementation
{
    public class ProductService : IProduct
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductService> _logger;
        private readonly CurrentUserService _currentUserService;

        public ProductService(AppDbContext context, ILogger<ProductService> logger, CurrentUserService currentUserService)
        {
            _context = context;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public async Task<ResponseDto<bool>> CreateProduct(CreateProductDto createProductDto)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Name.ToLower() == createProductDto.Name.ToLower());

            if (product != null)
            {
                return ResponseDto<bool>.Failure(
                    message: "Product already exists",
                    errors: new List<ApiError>
                    {
                        new ApiError { ErrorCode = "PRODUCT_EXIST", ErrorMessage = "Product already exists" }
                    }
                );
            }

            var userId = _currentUserService.GetCurrentUserId();

            var newProduct = new Product
            {
                Name = createProductDto.Name,
                Description = createProductDto.Description,
                Price = createProductDto.Price,
                Stock = createProductDto.Stock,
                ImageUrl = createProductDto.ImageUrl,
                IsActive = createProductDto.IsActive,
                CategoryId = createProductDto.CategoryId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userId ,
            };

            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Product created successfully with ID: {ProductId}", newProduct.Id);
            return ResponseDto<bool>.SuccessResponse(true, "Product created successfully");
        }

        public async Task<ResponseDto<bool>> DeleteProduct(Guid id)
        {
            var userId = _currentUserService.GetCurrentUserId();

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id && p.CreatedBy == userId);

            if (product == null)
            {
                return ResponseDto<bool>.Failure(
                    message: "Product not found or you don't have permission to delete it",
                    errors: new List<ApiError>
                    {
                new ApiError { ErrorCode = "PRODUCT_NOT_FOUND_OR_UNAUTHORIZED", ErrorMessage = $"Product with ID {id} not found or not owned by user." }
                    }
                );
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            _logger.LogInformation("User {UserId} deleted their product with ID: {ProductId}", userId, id);

            return ResponseDto<bool>.SuccessResponse(true, "Product deleted successfully");
        }


        public async Task<ResponseDto<List<ProductDto>>> GetAllProducts()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Stock = p.Stock,
                    ImageUrl = p.ImageUrl,
                    IsActive = p.IsActive,
                    CategoryId = p.CategoryId,
                    CreatedBy = p.CreatedBy,
                    UpdatedBy = p.UpdatedBy,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                })
                .ToListAsync();
            return ResponseDto<List<ProductDto>>.SuccessResponse(products, "Products retrieved successfully");
        }

        public async Task<ResponseDto<ProductDto>> GetProductById(Guid id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.Id == id)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Stock = p.Stock,
                    ImageUrl = p.ImageUrl,
                    IsActive = p.IsActive,
                    CategoryId = p.CategoryId,
                    CreatedBy = p.CreatedBy,
                    UpdatedBy = p.UpdatedBy,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                })
                .FirstOrDefaultAsync();

            if (product == null)
                {
                return ResponseDto<ProductDto>.Failure(
                    message: "Product not found",
                    errors: new List<ApiError>
                    {
                        new ApiError { ErrorCode = "PRODUCT_NOT_FOUND", ErrorMessage = $"Product with ID {id} not found." }
                    }
                );
            }
            return ResponseDto<ProductDto>.SuccessResponse(product, "Product retrieved successfully");
        }

        public async Task<ResponseDto<bool>> UpdateProduct(Guid id, CreateProductDto updateProductDto)
        {
            var userId = _currentUserService.GetCurrentUserId();

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id && p.CreatedBy == userId);

            if (product == null)
            {
                return ResponseDto<bool>.Failure(
                    message: "Product not found or you don't have permission to update it",
                    errors: new List<ApiError>
                    {
                new ApiError
                {
                    ErrorCode = "PRODUCT_NOT_FOUND_OR_UNAUTHORIZED",
                    ErrorMessage = $"Product with ID {id} not found or not owned by current user."
                }
                    }
                );
            }

            product.Name = updateProductDto.Name;
            product.Description = updateProductDto.Description;
            product.Price = updateProductDto.Price;
            product.Stock = updateProductDto.Stock;
            product.ImageUrl = updateProductDto.ImageUrl;
            product.IsActive = updateProductDto.IsActive;
            product.CategoryId = updateProductDto.CategoryId;
            product.UpdatedAt = DateTime.UtcNow;
            product.UpdatedBy = userId;

            await _context.SaveChangesAsync();

            _logger.LogInformation("User {UserId} updated product with ID: {ProductId}", userId, id);

            return ResponseDto<bool>.SuccessResponse(true, "Product updated successfully");
        }

    }
}
