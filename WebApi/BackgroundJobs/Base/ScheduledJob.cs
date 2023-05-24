namespace WebApi.BackgroundJobs;

public abstract class ScheduledJob : BackgroundService
{

    protected readonly ILogger<ScheduledJob> _logger;
    protected int executionCount = 0;
    protected Timer _timer = null;
    protected TimeSpan _timeSpan { get; set; } = TimeSpan.FromSeconds(5);

    public ScheduledJob(ILogger<ScheduledJob> logger)
    {
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service running.");

        _timer = new Timer(Job, null, TimeSpan.Zero,
            _timeSpan);

        return Task.CompletedTask;
    }

    protected abstract void Job(object state);

    public override Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _timer?.Dispose();
    }
}