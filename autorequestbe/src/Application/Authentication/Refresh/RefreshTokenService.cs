using System.Security.Cryptography;
using System.Text;
using Application.Authentication.Abstractions;

namespace Application.Authentication.Refresh;

public sealed class RefreshTokenService : IRefreshTokenService
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ITokenService _tokenService;

    public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository, ITokenService tokenService)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _tokenService = tokenService;
    }

    public async Task<RefreshResult?> RefreshAsync(string? refreshToken, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            return null;

        var now = DateTimeOffset.UtcNow;

        // Hash the incoming raw refresh token.
        var currentTokenHash = HashToken(refreshToken);

        // Retrieve its session and associated user.
        var session = await _refreshTokenRepository.GetByTokenHashAsync(currentTokenHash, ct);

        if (session is null)
            return null;

        // Reject expired or revoked sessions.
        if (session.RevokedAtUtc is not null ||
            session.ExpiresAtUtc <= now)
        {
            return null;
        }

        // Reject users who can no longer access the system.
        if (!session.User.CanAccessSystem)
            return null;

        // Prepare replacement tokens.
        var newAccessToken = _tokenService.GenerateAccessToken(session.User);

        var newRefreshToken = _tokenService.GenerateRefreshToken();

        var newTokenHash = HashToken(newRefreshToken.Value);

        // Atomically replace the old refresh-token hash.
        // Only one concurrent request can succeed.
        var rotated = await _refreshTokenRepository.TryRotateAsync(
            session.Id,
            currentTokenHash,
            newTokenHash,
            newRefreshToken.ExpiresAtUtc,
            now,
            ct);

        if (!rotated)
            return null;

        return new RefreshResult(
            newAccessToken.Value,
            newRefreshToken.Value,
            newAccessToken.ExpiresAtUtc,
            newRefreshToken.ExpiresAtUtc);
    }

    private static string HashToken(string token)
    {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));

        return Convert.ToHexString(bytes);
    }
}