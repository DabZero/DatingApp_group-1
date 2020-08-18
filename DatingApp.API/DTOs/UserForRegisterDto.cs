using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.DTOs
{
    /// <summary>
    /// Accepts a Username & Password for Controller parameters passed in a Post body
    /// For the purpose of registering new users
    /// </summary>
    public class UserForRegisterDto
    {
        [Required]
        [MinLength(4, ErrorMessage = "Must be at least 4 characters")]
        public string Username { get; set; }


        [Required]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Must be between 4-50 characters")]
        public string Password { get; set; }
    }
}