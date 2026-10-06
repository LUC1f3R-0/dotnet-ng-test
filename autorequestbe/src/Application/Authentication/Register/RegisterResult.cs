using Domain.Enums;

public sealed record RegisterResult(
    Guid UserUuid,
    string Name,
    string Email,
    StatusType Status,
    bool IsVerified
);