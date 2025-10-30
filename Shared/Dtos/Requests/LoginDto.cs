using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Shop_Api.Dtos.Requests
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
