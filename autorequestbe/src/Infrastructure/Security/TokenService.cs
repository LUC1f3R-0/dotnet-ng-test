using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Authentication.Abstractions;
using Domain.Entities;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Security;

public sealed class TokenService : ITokenService
{
    private readonly JwtOptions _options;

    public TokenService(IOptions<JwtOptions> options)
    {
        _options = options.Value;

        if (string.IsNullOrWhiteSpace(_options.Issuer) ||
            string.IsNullOrWhiteSpace(_options.Audience) ||
            Encoding.UTF8.GetByteCount(_options.SigningKey) < 32 ||
            _options.AccessTokenMinutes <= 0 ||
            _options.RefreshTokenDays <= 0)
        {
            throw new InvalidOperationException("JWT configuration is missing or invalid.");
        }
    }

    public IssuedToken GenerateAccessToken(User user)
    {
        var now = DateTimeOffset.UtcNow;

        var expiresAt = now.AddMinutes(_options.AccessTokenMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.UserUuid.ToString()),

            new(JwtRegisteredClaimNames.Email, user.Email),

            new(ClaimTypes.Name, user.Name),

            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        if (user.Role is not null)
        {
            claims.Add(new Claim(ClaimTypes.Role, user.Role.Value.ToString()));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SigningKey));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var jwt = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            notBefore: now.UtcDateTime,
            expires: expiresAt.UtcDateTime,
            signingCredentials: credentials);

        var token = new JwtSecurityTokenHandler().WriteToken(jwt);

        return new IssuedToken(token, expiresAt);
    }

    public IssuedToken GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);

        var token = Convert.ToBase64String(bytes);

        var expiresAt = DateTimeOffset.UtcNow.AddDays(_options.RefreshTokenDays);

        return new IssuedToken(token, expiresAt);
    }
}