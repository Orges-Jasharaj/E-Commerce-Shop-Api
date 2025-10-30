namespace E_Commerce_Shop_Api.Dtos.Requests
{
    public class CreateOrderDto
    {
        public List<Guid> CartItemIds { get; set; } = new();
    }
}
