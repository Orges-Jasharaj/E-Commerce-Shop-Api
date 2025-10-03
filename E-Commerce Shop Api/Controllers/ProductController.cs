using E_Commerce_Shop_Api.Data.Models;
using E_Commerce_Shop_Api.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Shop_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProduct _productService;

        public ProductController(IProduct productService)
        {
            _productService = productService;
        }

        [HttpPost("create-product")]
        [Authorize(Roles = $"{RoleTypes.Admin},{RoleTypes.SuperAdmin},{RoleTypes.Seller}")]
        public async Task<IActionResult> CreateProduct([FromBody] Dtos.Requests.CreateProductDto createProductDto)
        {
            var result = await _productService.CreateProduct(createProductDto);
            return Ok(result);
        }

        [HttpGet("get-all-products")]
        [Authorize(Roles = $"{RoleTypes.Admin},{RoleTypes.SuperAdmin},{RoleTypes.Seller},{RoleTypes.User}")]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _productService.GetAllProducts();
            return Ok(result);
        }


        [HttpGet("get-product-by-id/{productId}")]
        [Authorize(Roles = $"{RoleTypes.Admin},{RoleTypes.SuperAdmin},{RoleTypes.Seller},{RoleTypes.User}")]
        public async Task<IActionResult> GetProductById([FromRoute] Guid productId)
        {
            var result = await _productService.GetProductById(productId);
            return Ok(result);
        }

        [HttpDelete("delete-product/{productId}")]
        [Authorize(Roles = $"{RoleTypes.Admin},{RoleTypes.SuperAdmin},{RoleTypes.Seller}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid productId)
        {
            var result = await _productService.DeleteProduct(productId);
            return Ok(result);
        }

        [HttpPut("update-product/{productId}")]
        [Authorize(Roles = $"{RoleTypes.Admin},{RoleTypes.SuperAdmin},{RoleTypes.Seller}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] Guid productId, [FromBody] Dtos.Requests.CreateProductDto updateProductDto)
        {
            var result = await _productService.UpdateProduct(productId, updateProductDto);
            return Ok(result);
        }
    }
}
