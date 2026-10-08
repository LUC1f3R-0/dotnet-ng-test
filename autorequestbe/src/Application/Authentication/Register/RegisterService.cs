using API.Register.Models;
using Application.Authentication.Abstractions;
using Application.Exceptions;
using Domain.Entities;

namespace Application.Authentication.Register;

public sealed class RegisterService : IRegisterService
{
    private readonly IAuthRepository _authRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterService(IAuthRepository authRepository, IPasswordHasher passwordHasher)
    {
        _authRepository = authRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<RegisterResult> RegisterAsync(RegisterInput input, CancellationToken ct = default)
    {
        if (input.Pass != input.ConfirmPass)
        {
            throw new ValidationException("Passwords do not match.");
        }

        var emailExists = await _authRepository.EmailExistsAsync(input.Email, ct);

        if (emailExists)
        {
            throw new ConflictException("Email already exists.");
        }

        var hashedPassword = _passwordHasher.Hash(input.Pass);

        var user = new User
        {
            Name = input.Name ?? string.Empty,
            Email = input.Email,
            PasswordHash = hashedPassword
        };

        var createdUser = await _authRepository.AddUserAsync(user, ct);

        return ToResult(createdUser);
    }

    private static RegisterResult ToResult(User user)
    {
        return new RegisterResult(
            user.UserUuid,
            user.Name,
            user.Email,
            user.Status,
            user.IsVerified
        );
    }
}