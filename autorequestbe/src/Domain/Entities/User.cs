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

    // null = no role yet (new user); an admin sets it to User or Admin
    public RoleType? Role { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAtUtc { get; private set; } = DateTimeOffset.UtcNow;

    public ICollection<Session> Sessions { get; set; } = new List<Session>();

    // Can log in at all (users without a role included)
    public bool CanAccessSystem =>
        IsVerified
        && Status == StatusType.Active;

    // Can use the advanced features (a role was granted by an admin)
    public bool HasFullAccess =>
        CanAccessSystem
        && Role is not null;

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