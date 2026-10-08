using API.Register.Models;
using Application.Authentication.Abstractions;
using Application.Exceptions;
using Domain.Entities;

namespace Application.Authentication.Login;

public sealed class LoginService : ILoginService
{
    private readonly IAuthRepository _authRepository;
    private readonly IPasswordHasher _passwordHasher;
    
    public LoginService(IAuthRepository repository, IPasswordHasher passwordHasher)
    {
        _authRepository = repository;
        _passwordHasher = passwordHasher; 
    }

    public async Task<LoginResult> LoginAsync(LoginInput input, CancellationToken ct = default)
    {
        var user = await _authRepository.GetUserByEmailAsync(input.Email, ct);

        if (user is null)
        {
            throw new UnauthorizedException("Invalid email or password.");
        }

       if (!_passwordHasher.Verify(input.Password, user.PasswordHash))
        {
            throw new UnauthorizedException("Invalid email or password.");
        }

        return ToResult(user);
    }

    private static LoginResult ToResult(User user)
    {
        return new LoginResult(
            user.UserUuid,
            user.Name,
            user.Email,
            user.Status,
            user.IsVerified
        );
    }
}