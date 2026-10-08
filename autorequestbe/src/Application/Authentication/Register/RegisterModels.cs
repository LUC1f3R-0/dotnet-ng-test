using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace API.Register.Models;

public sealed record RegisterInput
(
    string? Name,

    [Required]
    [EmailAddress]
    string Email,

    [Required]
    string Pass,

    [Required]
    string ConfirmPass
);

public sealed record RegisterResult
(
    Guid UserUuid,
    string Name,
    string Email,
    StatusType Status,
    bool IsVerified
);