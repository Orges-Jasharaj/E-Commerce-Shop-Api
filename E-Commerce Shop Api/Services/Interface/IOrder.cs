using E_Commerce_Shop_Api.Dtos.Requests;
using E_Commerce_Shop_Api.Dtos.Responses;

namespace E_Commerce_Shop_Api.Services.Interface
{
    public interface IOrder
    {
        Task<ResponseDto<bool>> CreateOrderAsync(CreateOrderDto dto);
        Task<ResponseDto<bool>> UpdateOrderAsync(Guid id, UpdateOrderDto dto);
        Task<ResponseDto<bool>> DeleteOrderAsync(Guid id);
        Task<ResponseDto<OrderDto>> GetOrderByIdAsync(Guid id);
        Task<ResponseDto<List<OrderDto>>> GetOrdersByUserAsync();
    }
}
