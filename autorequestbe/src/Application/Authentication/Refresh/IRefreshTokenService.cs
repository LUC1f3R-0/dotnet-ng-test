namespace Application.Authentication.Refresh;

public sealed record RefreshResult
(
    string AccessToken,
    string RefreshToken,
    DateTimeOffset AccessTokenExpiresAtUtc,
    DateTimeOffset RefreshTokenExpiresAtUtc
);

public interface IRefreshTokenService
{
    Task<RefreshResult?> RefreshAsync(string? refreshToken, CancellationToken ct = default);
}