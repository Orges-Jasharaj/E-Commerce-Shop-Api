using E_Commerce_Shop_Api.Dtos.Requests;
using E_Commerce_Shop_Api.Services.Interface;
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
        public async Task<IActionResult> AddCartItem([FromBody] CreateCartItemDto dto)
        {
            return Ok(await _cartItemService.AddCartItem(dto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCartItem(Guid id, [FromBody] UpdateCartItemDto dto)
        {
            return Ok(await _cartItemService.UpdateCartItem(id, dto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartItem(Guid id)
        {
            return Ok(await _cartItemService.DeleteCartItem(id));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCartItemById(Guid id)
        {
            return Ok(await _cartItemService.GetCartItemById(id));
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetAllCartItemsByUser()
        {
            return Ok(await _cartItemService.GetAllCartItemsByUser());
        }

    }
}
