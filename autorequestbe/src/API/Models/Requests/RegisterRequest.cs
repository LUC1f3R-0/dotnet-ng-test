using System.ComponentModel.DataAnnotations;

namespace API.Models.Requests;

public sealed class RegisterRequest
{
    [MaxLength(100, ErrorMessage = "Name is too long.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Email format is invalid")]
    [MaxLength(100, ErrorMessage = "Email is too long.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
    [MaxLength(72, ErrorMessage = "Password is too long.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
    [MaxLength(72, ErrorMessage = "Password is too long.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}