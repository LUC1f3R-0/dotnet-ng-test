using Domain.Entities;

namespace Application.Authentication.Abstractions;

public interface IAuthRepository
{
    // Task<User?> GetUserByEmailAsync(string email, CancellationToken ct = default);

    Task<bool> EmailExistsAsync(string email, CancellationToken ct = default);

    Task<User> AddUserAsync(User user, CancellationToken ct = default);

    // Task UpdateUserAsync(User user, CancellationToken ct = default);
}