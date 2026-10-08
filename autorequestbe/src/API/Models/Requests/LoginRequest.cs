using System.ComponentModel.DataAnnotations;

namespace API.Models.Requests;

public sealed class LoginRequest
{
    [Required(ErrorMessage = "Email is Required.")]
    [EmailAddress(ErrorMessage = "Invalid Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is Required")]
    public string Password { get; set; } = string.Empty;
}