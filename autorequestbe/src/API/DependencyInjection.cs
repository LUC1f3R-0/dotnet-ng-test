using API.Extentions.Cors;
using API.Exceptions;
using System.Text;
using API.Authentication;
using Infrastructure.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace API;

public static class DependencyInjection
{
    public static IServiceCollection AddAPI(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        services.AddFrontendCors(configuration);

        services.AddScoped<AuthCookieService>();

        var jwt = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>() ?? throw new InvalidOperationException("JWT configuration is missing.");

        if (string.IsNullOrWhiteSpace(jwt.Issuer) | string.IsNullOrWhiteSpace(jwt.Audience) || Encoding.UTF8.GetByteCount(jwt.SigningKey) < 32 | jwt.AccessTokenMinutes <= 0 ||jwt.RefreshTokenDays <= 0)
        {
            throw new InvalidOperationException("JWT configuration is missing or invalid.");
        }

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.MapInboundClaims = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwt.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwt.Audience,

                    ValidateIssuerSigningKey = true,

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SigningKey)),

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,

                    NameClaimType = "sub",
                    RoleClaimType = System.Security.Claims.ClaimTypes.Role
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["accessToken"];

                        return Task.CompletedTask;
                    }
                };
            });
        services.AddAuthorization();

        return services;
                
    }

    public static WebApplication UseAPI(this WebApplication app)
        {
            app.UseExceptionHandler();
            app.UseRouting();
            app.UseCors(CorsExtensions.PolicyName);
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
    
            return app;
        }
}