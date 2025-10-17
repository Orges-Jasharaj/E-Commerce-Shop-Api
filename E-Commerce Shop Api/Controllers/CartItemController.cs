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
    public class CartItemController : ControllerBase
    {

        private readonly ICartItem _cartItemService;

        public CartItemController(ICartItem cartItemService)
        {
            _cartItemService = cartItemService;
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleTypes.User}")]
        public async Task<IActionResult> AddCartItem([FromBody] CreateCartItemDto dto)
        {
            return Ok(await _cartItemService.AddCartItem(dto));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = $"{RoleTypes.User}")]
        public async Task<IActionResult> UpdateCartItem(Guid id, [FromBody] UpdateCartItemDto dto)
        {
            return Ok(await _cartItemService.UpdateCartItem(id, dto));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{RoleTypes.User}")]
        public async Task<IActionResult> DeleteCartItem(Guid id)
        {
            return Ok(await _cartItemService.DeleteCartItem(id));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = $"{RoleTypes.Admin},{RoleTypes.SuperAdmin},{RoleTypes.Seller}")]
        public async Task<IActionResult> GetCartItemById(Guid id)
        {
            return Ok(await _cartItemService.GetCartItemById(id));
        }

        [HttpGet("user")]
        [Authorize(Roles = $"{RoleTypes.User}")]
        public async Task<IActionResult> GetAllCartItemsByUser()
        {
            return Ok(await _cartItemService.GetAllCartItemsByUser());
        }

    }
}
