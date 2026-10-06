using Application.Authentication.Abstractions;
using Infrastructure.Email;
using Infrastructure.Initialization;
using Infrastructure.Options;
using Infrastructure.Persistence;
using Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEmail();
        services.AddPersistence(configuration);
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddHostedService<StartupInitializer>();
        services.Configure<SmtpOptions>(configuration.GetSection(SmtpOptions.SectionName));

        return services;
    }
}
