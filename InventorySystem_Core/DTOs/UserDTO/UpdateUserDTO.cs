using System.ComponentModel.DataAnnotations;

namespace InventorySystem_Core.DTOs.UserDTO
{
    public class UpdateUserDTO
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [StringLength(100, ErrorMessage = "Email address cannot exceed 100 characters.")]
        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = "User";
    }
}
