using E_Commerce_Shop_Api.Dtos.Requests;
using E_Commerce_Shop_Api.Dtos.Responses;

namespace E_Commerce_Shop_Api.Services.Interface
{
    public interface IProduct
    {
        Task<ResponseDto<bool>> CreateProduct(CreateProductDto createProductDto);
        Task<ResponseDto<bool>> UpdateProduct(Guid id, CreateProductDto updateProductDto);
        Task<ResponseDto<bool>> DeleteProduct(Guid id);
        Task<ResponseDto<ProductDto>> GetProductById(Guid id);
        Task<ResponseDto<List<ProductDto>>> GetAllProducts();
    }
}
