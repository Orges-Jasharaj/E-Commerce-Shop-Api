using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Shop_Api.Dtos.Requests
{
    public class UpdateUserDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
    }
}
