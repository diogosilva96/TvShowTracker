using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TvShowTracker.Domain.Models
{
    public class RegisterUserModel
    {
        [Required(ErrorMessage = $"{nameof(FirstName)} is required")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = $"{nameof(LastName)} is required")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = $"{nameof(Password)} is required")]
        public string? Password { get; set; }

        [Required(ErrorMessage = $"{nameof(Email)} is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = $"{nameof(GrantGdprConsent)} is required")]
        public bool? GrantGdprConsent { get; set; }

    }
}
