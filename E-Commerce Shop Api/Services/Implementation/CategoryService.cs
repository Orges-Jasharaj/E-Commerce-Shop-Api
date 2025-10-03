using E_Commerce_Shop_Api.Data;
using E_Commerce_Shop_Api.Data.Models;
using E_Commerce_Shop_Api.Dtos.Requests;
using E_Commerce_Shop_Api.Dtos.Responses;
using E_Commerce_Shop_Api.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace E_Commerce_Shop_Api.Services.Implementation
{
    public class CategoryService : ICategory
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CategoryService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CategoryService(AppDbContext context, ILogger<CategoryService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<ResponseDto<bool>> CreateCategory(CreateCategoryDto createCategoryDto)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name.ToLower() == createCategoryDto.Name.ToLower());

            if (category != null)
            {
                return ResponseDto<bool>.Failure(
                    message: "Category already exists",
                    errors: new List<ApiError>
                    {
                new ApiError { ErrorCode = "CATEGORY_EXIST", ErrorMessage = "Category already exists" }
                    }
                );
            }

            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

            var newCategory = new Category
            {
                Name = createCategoryDto.Name,
                Description = createCategoryDto.Description,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userName ?? "System"
            };

            _context.Categories.Add(newCategory);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Category created successfully with ID: {CategoryId}", newCategory.Id);

            return ResponseDto<bool>.SuccessResponse(true, "Category created successfully");
        }

        public async Task<ResponseDto<bool>> DeleteCategory(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return ResponseDto<bool>.Failure(
                    message: "Category not found",
                    errors: new List<ApiError>
                    {
                        new ApiError { ErrorCode = "CATEGORY_NOT_FOUND", ErrorMessage = $"Category with ID {id} not found." }
                    }
                );
            }
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Category with ID: {CategoryId} deleted successfully", id);
            return ResponseDto<bool>.SuccessResponse(true, "Category deleted successfully");
        }

        public async Task<ResponseDto<List<CategoryDto>>> GetAllCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            var categoryDtos = categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                CreatedBy = c.CreatedBy,
                CreatedAt = c.CreatedAt,
                UpdatedBy = c.UpdatedBy,
                UpdatedAt = c.UpdatedAt,
                Products = c.Products
            }).ToList();
            return ResponseDto<List<CategoryDto>>.SuccessResponse(categoryDtos, "Categories retrieved successfully");
        }

        public async Task<ResponseDto<CategoryDto>> GetCategoryById(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return ResponseDto<CategoryDto>.Failure(
                    message: "Category not found",
                    errors: new List<ApiError>
                    {
                        new ApiError { ErrorCode = "CATEGORY_NOT_FOUND", ErrorMessage = $"Category with ID {id} not found." }
                    }
                );
            }
            var categoryDto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                CreatedBy = category.CreatedBy,
                CreatedAt = category.CreatedAt,
                UpdatedBy = category.UpdatedBy,
                UpdatedAt = category.UpdatedAt,
                Products = category.Products
            };
            return ResponseDto<CategoryDto>.SuccessResponse(categoryDto, "Category retrieved successfully");
        }

        public async Task<ResponseDto<bool>> UpdateCategory(Guid id, CreateCategoryDto updateCategoryDto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return ResponseDto<bool>.Failure(
                    message: "Category not found",
                    errors: new List<ApiError>
                    {
                        new ApiError { ErrorCode = "CATEGORY_NOT_FOUND", ErrorMessage = $"Category with ID {id} not found." }
                    }
                );
            }
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = _httpContextAccessor.HttpContext?.User?.Identity?.Name;



            category.Name = updateCategoryDto.Name;
            category.Description = updateCategoryDto.Description;
            category.UpdatedAt = DateTime.UtcNow;
            category.UpdatedBy = userName ?? "System";
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Category with ID: {CategoryId} updated successfully", id);
            return ResponseDto<bool>.SuccessResponse(true, "Category updated successfully");
        }
    }
}
