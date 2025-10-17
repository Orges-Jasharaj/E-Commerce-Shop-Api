using E_Commerce_Shop_Api.Data.Models;
using E_Commerce_Shop_Api.Dtos.Requests;
using E_Commerce_Shop_Api.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Shop_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrder _orderService;

        public OrderController(IOrder orderService)
        {
            _orderService = orderService;
        }


        [Authorize(Roles = $"{RoleTypes.User}")]
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
        {
            var response = await _orderService.CreateOrderAsync(dto);
            return Ok(response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = $"{RoleTypes.Admin},{RoleTypes.SuperAdmin},{RoleTypes.Seller}")]
        public async Task<IActionResult> UpdateOrder(Guid id, [FromBody] UpdateOrderDto dto)
        {
            var response = await _orderService.UpdateOrderAsync(id, dto);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{RoleTypes.Admin},{RoleTypes.SuperAdmin},{RoleTypes.Seller},{RoleTypes.User}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var response = await _orderService.DeleteOrderAsync(id);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = $"{RoleTypes.Admin},{RoleTypes.SuperAdmin},{RoleTypes.Seller}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var response = await _orderService.GetOrderByIdAsync(id);
            return Ok(response);
        }

        [HttpGet("user")]
        [Authorize(Roles = $"{RoleTypes.User}")]
        public async Task<IActionResult> GetOrdersByUser()
        {
            var response = await _orderService.GetOrdersByUserAsync();
            return Ok(response);
        }
    }
}
