using E_Commerce_Shop_Api.Data;
using E_Commerce_Shop_Api.Data.Models;
using E_Commerce_Shop_Api.Dtos.Requests;
using E_Commerce_Shop_Api.Dtos.Responses;
using E_Commerce_Shop_Api.Hubs;
using E_Commerce_Shop_Api.Services.Interface;
using Hangfire;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Shop_Api.Services.Implementation
{
    public class OrderService : IOrder
    {
        private readonly AppDbContext _context;
        private readonly CurrentUserService _currentUserService;
        private readonly ILogger<OrderService> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;

        public OrderService(AppDbContext context, CurrentUserService currentUserService, ILogger<OrderService> logger,IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _currentUserService = currentUserService;
            _logger = logger;
            _hubContext = hubContext;
        }

        [AutomaticRetry(Attempts = 3)]
        public async Task<ResponseDto<bool>> CreateOrderAsync(CreateOrderDto dto)
        {
            var userId = _currentUserService.GetCurrentUserId();

            var cartItems = await _context.CartItems
                .Include(c => c.Product)
                .Where(c => dto.CartItemIds.Contains(c.Id) && c.UserId == userId)
                .ToListAsync();

            if (!cartItems.Any())
                return ResponseDto<bool>.Failure("No valid cart items found.");

            var order = new Order
            {
                UserId = userId,
                CreatedBy = userId,
                TotalAmount = cartItems.Sum(ci => ci.Product.Price * ci.Quantity),
                OrderItems = cartItems.Select(ci => new OrderItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.Product.Price
                }).ToList() 
            };

            await _context.Orders.AddAsync(order);
            _context.CartItems.RemoveRange(cartItems); 
            await _context.SaveChangesAsync();

            _logger.LogInformation("Order {OrderId} created successfully for user {UserId}.", order.Id, userId);

            await _hubContext.Clients.All.SendAsync("ReceiveNotification", $"New order created by user {userId}");

            return ResponseDto<bool>.SuccessResponse(true, "Order created successfully.");
        }

        [AutomaticRetry(Attempts = 3)]
        public async Task<ResponseDto<bool>> UpdateOrderAsync(Guid id, UpdateOrderDto dto)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return ResponseDto<bool>.Failure("Order not found.");

            order.Status = dto.Status;
            order.UpdatedBy = _currentUserService.GetCurrentUserId();
            order.UpdatedAt = DateTime.UtcNow;

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Order {OrderId} status updated to {Status}.", id, dto.Status);

            return ResponseDto<bool>.SuccessResponse(true, "Order updated successfully.");
        }

        public async Task<ResponseDto<bool>> DeleteOrderAsync(Guid id)
        {
            var order = await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
                return ResponseDto<bool>.Failure("Order not found.");

            _context.OrderItems.RemoveRange(order.OrderItems);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Order {OrderId} deleted successfully.", id);

            return ResponseDto<bool>.SuccessResponse(true, "Order deleted successfully.");
        }

        public async Task<ResponseDto<OrderDto>> GetOrderByIdAsync(Guid id)
        {
            var userId = _currentUserService.GetCurrentUserId();

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

            if (order == null)
                return ResponseDto<OrderDto>.Failure("Order not found.");

            var orderDto = new OrderDto
            {
                Id = order.Id,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                CreatedAt = order.CreatedAt,
                Items = order.OrderItems.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product.Name,
                    Price = oi.UnitPrice,
                    Quantity = oi.Quantity
                }).ToList()
            };

            return ResponseDto<OrderDto>.SuccessResponse(orderDto, "Order retrieved successfully.");
        }

        public async Task<ResponseDto<List<OrderDto>>> GetOrdersByUserAsync()
        {
            var userId = _currentUserService.GetCurrentUserId();

            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .ToListAsync();

            var orderDtos = orders.Select(o => new OrderDto
            {
                Id = o.Id,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                CreatedAt = o.CreatedAt,
                Items = o.OrderItems.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product.Name,
                    Price = oi.UnitPrice,
                    Quantity = oi.Quantity
                }).ToList()
            }).ToList();

            return ResponseDto<List<OrderDto>>.SuccessResponse(orderDtos, "Orders retrieved successfully.");
        }
    }
}
