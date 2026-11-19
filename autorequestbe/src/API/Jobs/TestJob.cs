using Quartz;

namespace API.Jobs;

public sealed class TestJob : IJob
{
    public ValueTask Execute(IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Hello from Quartz.NET: {DateTime.Now:HH:mm:ss}");
        return ValueTask.CompletedTask;
    }
}