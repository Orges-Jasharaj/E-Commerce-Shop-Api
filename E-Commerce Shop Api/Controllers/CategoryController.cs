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
    public class CategoryController : ControllerBase
    {
        private readonly ICategory _category;

        public CategoryController(ICategory category)
        {
            _category = category;
        }


        [HttpGet]
        [Authorize(Roles = $"{RoleTypes.Admin},{RoleTypes.SuperAdmin},{RoleTypes.Seller},{RoleTypes.User}")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _category.GetAllCategories();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = $"{RoleTypes.Admin},{RoleTypes.SuperAdmin},{RoleTypes.Seller},{RoleTypes.User}")]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            var category = await _category.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleTypes.Admin},{RoleTypes.SuperAdmin},{RoleTypes.Seller}")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            return Ok(await _category.CreateCategory(createCategoryDto));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{RoleTypes.Admin},{RoleTypes.SuperAdmin},{RoleTypes.Seller}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            return Ok(await _category.DeleteCategory(id));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = $"{RoleTypes.Admin},{RoleTypes.SuperAdmin},{RoleTypes.Seller}")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] CreateCategoryDto updateCategoryDto)
        {
            return Ok(await _category.UpdateCategory(id, updateCategoryDto));
        }


    }
}
