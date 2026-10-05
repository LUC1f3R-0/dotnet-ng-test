using Application.Authentication;
using Application.Authentication.Register;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IRegisterService, RegisterService>();

        return services;
    }
}