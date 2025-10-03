using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Shop_Api.Dtos.Requests
{
    public class CreateCategoryDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(300)]
        public string? Description { get; set; }
    }
}
