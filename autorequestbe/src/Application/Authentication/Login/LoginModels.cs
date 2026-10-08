using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace API.Register.Models;

public sealed record LoginInput
(
    [Required]
    [EmailAddress]
    string Email,

    [Required]
    string Password
);

public sealed record LoginResult
(
    Guid UserUuid,
    string Name,
    string Email,
    StatusType Status,
    bool IsVerified
);