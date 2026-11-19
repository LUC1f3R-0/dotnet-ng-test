using API.Jobs;
using Quartz;

namespace API.Extensions.Scheduling;

public static class QuartzExtensions
{
    public static IServiceCollection AddQuartzJobs(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartz(configuration.GetSection("Quartz"), q =>
            {
                q.ScheduleJob<TestJob>(trigger => trigger
                    .WithIdentity("test-job-trigger")
                    .StartNow()
                    .WithSimpleSchedule(schedule => schedule
                        .WithInterval(TimeSpan.FromSeconds(10))
                        .RepeatForever()
                    )
                );
            }
        );

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });

        return services;
    }
}