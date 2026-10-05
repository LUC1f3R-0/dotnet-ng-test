
using API.Extentions.Cors;
using API.Exceptions;

namespace API;

public static class DependencyInjection
{
    public static IServiceCollection AddAPI(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        services.AddFrontendCors(configuration);

        return services;
    }

    public static WebApplication UseAPI(this WebApplication app)
    {
        app.UseExceptionHandler();
        app.UseCors(CorsExtensions.PolicyName);
        app.MapControllers();

        return app;
    }
}