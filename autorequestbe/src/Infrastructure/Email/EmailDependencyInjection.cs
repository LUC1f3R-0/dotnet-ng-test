using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Email;

public static class EmailDependencyInjection
{
    public static IServiceCollection AddEmail(this IServiceCollection services)
    {
        services.AddTransient<SmtpConnectionValidator>();

        return services;
    }
}