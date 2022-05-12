using System.ComponentModel.DataAnnotations;

namespace TvShowTracker.Domain.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = $"{nameof(Email)} is required")]
        public string? Email { get; set; }
        [Required(ErrorMessage = $"{nameof(Password)} is required")]
        public string? Password { get; set; }
    }
}
