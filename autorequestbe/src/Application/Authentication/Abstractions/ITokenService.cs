using Domain.Entities;

namespace Application.Authentication.Abstractions;

public sealed record IssuedToken
(
    string Value,
    DateTimeOffset ExpiresAtUtc
);

public interface ITokenService
{
    IssuedToken GenerateAccessToken(User user);

    IssuedToken GenerateRefreshToken();
}