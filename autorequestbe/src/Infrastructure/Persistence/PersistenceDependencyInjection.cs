using Application.Authentication.Abstractions;
using Infrastructure.Options;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Infrastructure.Persistence;

public static class PersistenceDependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection(DatabaseOptions.SectionName).Get<DatabaseOptions>() ?? throw new InvalidOperationException("Database configuration section is missing.");

        if (string.IsNullOrWhiteSpace(options.Host))
            throw new InvalidOperationException("Database host is missing.");
        if (options.Port <= 0)
            throw new InvalidOperationException("Database port is missing or invalid.");
        if (string.IsNullOrWhiteSpace(options.Name))
            throw new InvalidOperationException("Database name is missing.");
        if (string.IsNullOrWhiteSpace(options.UserName))
            throw new InvalidOperationException("Database username is missing.");

        var connectionString = new NpgsqlConnectionStringBuilder
        {
            Host = options.Host,
            Port = options.Port,
            Database = options.Name,
            Username = options.UserName,
            Password = options.Password
        }.ConnectionString;

        services.AddDbContext<ApplicationDbContext>(o => o.UseNpgsql(connectionString));
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        return services;
    }
}