using E_Commerce_Shop_Api.Data;
using E_Commerce_Shop_Api.Data.Models;
using E_Commerce_Shop_Api.Dtos.Requests;
using E_Commerce_Shop_Api.Dtos.Responses;
using E_Commerce_Shop_Api.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace E_Commerce_Shop_Api.Services.Implementation
{
    public class CartItemService : ICartItem
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CartItemService> _logger;
        private readonly CurrentUserService _currentUserService;

        public CartItemService(AppDbContext context, CurrentUserService currentUserService, ILogger<CartItemService> logger)
        {
            _context = context;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        
        public async Task<ResponseDto<bool>> AddCartItem(CreateCartItemDto dto)
        {
            var userId = _currentUserService.GetCurrentUserId();

            var product = await _context.Products.FindAsync(dto.ProductId);
            if (product == null)
                return ResponseDto<bool>.Failure("Product not found.");

            var existing = await _context.CartItems
                .FirstOrDefaultAsync(x => x.ProductId == dto.ProductId && x.UserId == userId);

            if (existing != null)
            {
                existing.Quantity += dto.Quantity;
                _context.CartItems.Update(existing);
            }
            else
            {
                var cartItem = new CartItem
                {
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    UserId = userId
                };
                await _context.CartItems.AddAsync(cartItem);
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Cart item added/updated for user {UserId} and product {ProductId}", userId, dto.ProductId);
            return ResponseDto<bool>.SuccessResponse(true, "Cart item added successfully.");
        }

        public async Task<ResponseDto<bool>> UpdateCartItem(Guid id, UpdateCartItemDto dto)
        {
            var userId = _currentUserService.GetCurrentUserId();

            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (cartItem == null)
                return ResponseDto<bool>.Failure("Cart item not found.");

            cartItem.Quantity = dto.Quantity;
            _context.CartItems.Update(cartItem);

            await _context.SaveChangesAsync();
            _logger.LogInformation("Cart item {CartItemId} updated for user {UserId}", id, userId);
            return ResponseDto<bool>.SuccessResponse(true, "Cart item updated successfully.");
        }


        public async Task<ResponseDto<bool>> DeleteCartItem(Guid id)
        {
            var userId = _currentUserService.GetCurrentUserId();

            var item = await _context.CartItems
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (item == null)
                return ResponseDto<bool>.Failure("Cart item not found.");

            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Cart item {CartItemId} deleted for user {UserId}", id, userId);
            return ResponseDto<bool>.SuccessResponse(true, "Cart item deleted successfully.");
        }


        public async Task<ResponseDto<CartItemDto>> GetCartItemById(Guid id)
        {
            var userId = _currentUserService.GetCurrentUserId();

            var item = await _context.CartItems
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (item == null)
                return ResponseDto<CartItemDto>.Failure("Cart item not found.");

            var dto = new CartItemDto
            {
                Id = item.Id,
                ProductId = item.ProductId,
                ProductName = item.Product.Name,
                Price = item.Product.Price,
                Quantity = item.Quantity
            };

            return ResponseDto<CartItemDto>.SuccessResponse(dto, "Cart item retrieved successfully.");
        }


        public async Task<ResponseDto<List<CartItemDto>>> GetAllCartItemsByUser()
        {
            var userId = _currentUserService.GetCurrentUserId();

            var items = await _context.CartItems
                .Include(x => x.Product)
                .Where(x => x.UserId == userId)
                .Select(x => new CartItemDto
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    ProductName = x.Product.Name,
                    Price = x.Product.Price,
                    Quantity = x.Quantity
                }).ToListAsync();

            return ResponseDto<List<CartItemDto>>.SuccessResponse(items, "Cart items retrieved successfully.");
        }
    }
}
