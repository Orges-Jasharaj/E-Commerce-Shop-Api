using E_Commerce_Shop_Api.Dtos.Requests;
using E_Commerce_Shop_Api.Dtos.Responses;

namespace E_Commerce_Shop_Api.Services.Interface
{
    public interface ICategory
    {
        Task<ResponseDto<bool>> CreateCategory(CreateCategoryDto createCategoryDto);
        Task<ResponseDto<bool>> UpdateCategory(Guid id,CreateCategoryDto updateCategoryDto);
        Task<ResponseDto<bool>> DeleteCategory(Guid id);
        Task<ResponseDto<CategoryDto>> GetCategoryById(Guid id);
        Task<ResponseDto<List<CategoryDto>>> GetAllCategories();
    }
}
