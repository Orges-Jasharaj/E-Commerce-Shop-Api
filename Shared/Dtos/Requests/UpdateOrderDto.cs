using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Shop_Api.Dtos.Requests
{
    public class UpdateOrderDto
    {
        [Required(ErrorMessage = "Status is required.")]
        [RegularExpression("^(Pending|Shipped|Delivered|Cancelled)$",
            ErrorMessage = "Status must be one of: Pending, Shipped, Delivered, or Cancelled.")]
        public string Status { get; set; } = string.Empty;
    }
}
