using Application.Authentication.Abstractions;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public sealed class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly ApplicationDbContext _context;

    public RefreshTokenRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Session?> GetByTokenHashAsync(string tokenHash, CancellationToken ct = default)
    {
        return await _context.Set<Session>()
            .AsNoTracking()
            .Include(s => s.User)
            .SingleOrDefaultAsync(s => s.RefreshTokenHash == tokenHash, ct);
    }

    public async Task CreateAsync(Session session, CancellationToken ct = default)
    {
        await _context.Set<Session>().AddAsync(session, ct);

        await _context.SaveChangesAsync(ct);
    }

    public async Task<bool> TryRotateAsync(long sessionId, string currentTokenHash, string newTokenHash, DateTimeOffset newExpiresAtUtc, DateTimeOffset now, CancellationToken ct = default)
    {
        var affectedRows = await _context.Set<Session>()
            .Where(s =>
                s.Id == sessionId &&
                s.RefreshTokenHash == currentTokenHash &&
                s.RevokedAtUtc == null &&
                s.ExpiresAtUtc > now)
            .ExecuteUpdateAsync(
                setters => setters
                    .SetProperty(
                        s => s.RefreshTokenHash,
                        newTokenHash)
                    .SetProperty(
                        s => s.ExpiresAtUtc,
                        newExpiresAtUtc)
                    .SetProperty(
                        s => s.UpdatedAtUtc,
                        now),
                ct);

        return affectedRows == 1;
    }

    public async Task<bool> RevokeAsync(string tokenHash, DateTimeOffset now, CancellationToken ct = default)
    {
        var affectedRows = await _context.Set<Session>()
            .Where(s =>
                s.RefreshTokenHash == tokenHash &&
                s.RevokedAtUtc == null).ExecuteUpdateAsync(setters => setters.SetProperty(
                s => s.RevokedAtUtc, now).SetProperty(s => s.UpdatedAtUtc, now), ct);

        return affectedRows == 1;
    }
}