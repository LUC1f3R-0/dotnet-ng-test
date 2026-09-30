using Infrastructure.Email;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Initialization;

public class StartupInitializer : IHostedService
{
    private readonly SmtpConnectionValidator _smtpValidator;
    private readonly ILogger<StartupInitializer> _logger;

    public StartupInitializer(SmtpConnectionValidator smtpValidator, ILogger<StartupInitializer> logger)
    {
        _smtpValidator = smtpValidator;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        // await CheckDatabaseAsync(cancellationToken);
        await CheckSmtpAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private async Task CheckSmtpAsync()
    {
        _logger.LogInformation("Checking SMTP connection...");
        try
        {
            await _smtpValidator.ValidateAsync();
            _logger.LogInformation("SMTP connection successful.");
        }
        catch (Exception exception)
        {
            _logger.LogWarning(exception, "SMTP connection failed.");
        }
    }
}