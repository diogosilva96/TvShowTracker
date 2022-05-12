using System.ComponentModel.DataAnnotations;

namespace TvShowTracker.Domain.Models;

public class JwtConfiguration
{
    [Required(ErrorMessage = $"{nameof(Audience)} is required")]
    public string Audience { get; set; }

    [Required(ErrorMessage = $"{nameof(Issuer)} is required")]
    public string Issuer { get; set; }

    [Required(ErrorMessage = $"{nameof(Secret)} is required")]
    public string Secret { get; set; }

    public int TokenExpirationTimeInMinutes { get; set; }
}