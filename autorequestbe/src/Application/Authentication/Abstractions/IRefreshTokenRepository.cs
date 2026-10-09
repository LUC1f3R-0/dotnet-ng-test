
using Domain.Entities;

namespace Application.Authentication.Abstractions;

public interface IRefreshTokenRepository
{
    Task<Session?> GetByTokenHashAsync(string tokenHash, CancellationToken ct = default);

    Task CreateAsync(Session session, CancellationToken ct = default);

    Task<bool> TryRotateAsync(long sessionId, string currentTokenHash, string newTokenHash, DateTimeOffset newExpiresAtUtc, DateTimeOffset now, CancellationToken ct = default);

    Task<bool> RevokeAsync(string tokenHash, DateTimeOffset now, CancellationToken ct = default);
}