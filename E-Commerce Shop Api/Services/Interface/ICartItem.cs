using E_Commerce_Shop_Api.Dtos.Requests;
using E_Commerce_Shop_Api.Dtos.Responses;

namespace E_Commerce_Shop_Api.Services.Interface
{
    public interface ICartItem
    {
        Task<ResponseDto<bool>> AddCartItem(CreateCartItemDto createCartItemDto);
        Task<ResponseDto<bool>> UpdateCartItem(Guid id, UpdateCartItemDto updateCartItemDto);
        Task<ResponseDto<bool>> DeleteCartItem(Guid id);
        Task<ResponseDto<CartItemDto>> GetCartItemById(Guid id);
        Task<ResponseDto<List<CartItemDto>>> GetAllCartItemsByUser();
    }
}
