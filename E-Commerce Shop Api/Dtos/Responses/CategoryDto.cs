using E_Commerce_Shop_Api.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Shop_Api.Dtos.Responses
{
    public class CategoryDto
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(300)]
        public string? Description { get; set; }
        public string CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<Product>? Products { get; set; }
    }
}
