namespace Domain.Entities;

public class Session
{
    public long Id { get; set; }
    public Guid SessionUuid { get; set; }

    public long UserId { get; set; }
    public User User { get; set; } = null!;

    public string RefreshTokenHash { get; set; } = null!;
    public string IpAddress { get; set; } = string.Empty;
    public string DeviceId { get; set; } = string.Empty;
    public string DeviceName { get; set; } = string.Empty;

    public DateTimeOffset ExpiresAtUtc { get; set; }
    
    public DateTimeOffset? RevokedAtUtc { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset UpdatedAtUtc { get; set; }
}