namespace WebApi.BackgroundJobs;

using WebApi.Services;

public class CleanToken : ScheduledJob
{
    private readonly IServiceProvider _services;
    public CleanToken(ILogger<CleanToken> logger, IServiceProvider services) : base(logger)
    {
        _timeSpan = TimeSpan.FromMinutes(1);
        _services = services;
    }

    protected sealed override async void Job(object state)
    {
        var count = Interlocked.Increment(ref executionCount);
        var deleted = 0;

        using (var scope = _services.CreateScope())
        {
            var jwtService = scope.ServiceProvider.GetRequiredService<IJWTService>();

            var tokens = await jwtService.GetExpired();

            foreach (var token in tokens)
            {
                await jwtService.Delete(token);
                deleted++;
            }
        }
    }
}