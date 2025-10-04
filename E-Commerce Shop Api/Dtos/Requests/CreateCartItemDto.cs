namespace E_Commerce_Shop_Api.Dtos.Requests
{
    public class CreateCartItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

    }
}
