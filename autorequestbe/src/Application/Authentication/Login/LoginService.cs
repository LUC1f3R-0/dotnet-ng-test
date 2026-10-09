using System.Security.Cryptography;
using System.Text;
using API.Register.Models;
using Application.Authentication.Abstractions;
using Application.Exceptions;
using Domain.Entities;

namespace Application.Authentication.Login;

public sealed class LoginService : ILoginService
{
    private readonly IAuthRepository _authRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public LoginService(
        IAuthRepository authRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _authRepository = authRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<LoginResult> LoginAsync(
        LoginInput input,
        CancellationToken ct = default)
    {
        var email = input.Email.Trim().ToLowerInvariant();

        var user = await _authRepository.GetUserByEmailAsync(
            email, ct);

        if (user is null ||
            !_passwordHasher.Verify(
                input.Password,
                user.PasswordHash))
        {
            throw new UnauthorizedException(
                "Invalid email or password.");
        }

        if (!user.CanAccessSystem)
        {
            throw new UnauthorizedException(
                "Account is not verified or is inactive.");
        }

        var accessToken =
            _tokenService.GenerateAccessToken(user);

        var refreshToken =
            _tokenService.GenerateRefreshToken();

        var now = DateTimeOffset.UtcNow;

        var refreshTokenHash = Convert.ToHexString(
            SHA256.HashData(
                Encoding.UTF8.GetBytes(refreshToken.Value)));

        var session = new Session
        {
            UserId = user.Id,
            RefreshTokenHash = refreshTokenHash,
            CreatedAtUtc = now,
            UpdatedAtUtc = now,
            ExpiresAtUtc = refreshToken.ExpiresAtUtc
        };

        // Persist the session before returning tokens.
        await _refreshTokenRepository.CreateAsync(session, ct);

        return new LoginResult(
            user.UserUuid,
            user.Name,
            user.Email,
            user.Status,
            user.IsVerified,
            user.Role,
            accessToken.Value,
            refreshToken.Value,
            accessToken.ExpiresAtUtc,
            refreshToken.ExpiresAtUtc
        );
    }
}