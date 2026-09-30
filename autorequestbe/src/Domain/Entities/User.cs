using Domain.Enums;

namespace Domain.Entities;

public class User
{
    public long Id { get; set; }
    public Guid UserUuid { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public StatusType Status { get; private set; } = StatusType.Deactivated;

    public bool IsVerified { get; private set; }

    public bool IsProfileComplete =>
        !string.IsNullOrWhiteSpace(Name)
        && !string.IsNullOrWhiteSpace(Email);

    public bool CanAccessSystem =>
        IsVerified
        && IsProfileComplete
        && Status == StatusType.Active;

    public RoleType Role { get; set; } = RoleType.User;

    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset UpdatedAtUtc { get; private set; }

    public ICollection<Session> Sessions { get; set; } = new List<Session>();
    public string TestColumn { get; set; } = string.Empty;

    public void Verify()
    {
        IsVerified = true;
        UpdatedAtUtc = DateTimeOffset.UtcNow;
    }

    public void Deactivate()
    {
        Status = StatusType.Deactivated;
        UpdatedAtUtc = DateTimeOffset.UtcNow;
    }

    public void Activate()
    {
        Status = StatusType.Active;
        UpdatedAtUtc = DateTimeOffset.UtcNow;
    }
}