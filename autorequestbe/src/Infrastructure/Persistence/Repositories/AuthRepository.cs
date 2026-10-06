using Application.Authentication.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;


public class AuthRepository : IAuthRepository
{
    private ApplicationDbContext _context;

    public AuthRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken ct = default)
    {
        return await _context.Users.AnyAsync(u => u.Email == email, ct);
    }

    public async Task<User> AddUserAsync(User user, CancellationToken ct = default)
    {
        await _context.Users.AddAsync(user, ct);
        await _context.SaveChangesAsync(ct);

        string[] input = { user.Name, user.Email };

        foreach (var value in input)
        {
            Console.WriteLine(value);
        }

        Console.WriteLine();
        return user;
    }
}