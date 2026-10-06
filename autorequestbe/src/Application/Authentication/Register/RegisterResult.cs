using Domain.Enums;

namespace Application.Authentication.Register.Result;

public sealed record RegisterResult(
    Guid UserUuid,
    string Name,
    string Email,
    StatusType Status,
    bool IsVerified
);