namespace HotelsBookingWebApi.Services;

/// <summary>
/// Background service that runs daily to clean up expired bookings
/// </summary>
public class BookingCleanupBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<BookingCleanupBackgroundService> _logger;
    private readonly TimeSpan _executionTime;

    public BookingCleanupBackgroundService(IServiceProvider serviceProvider, ILogger<BookingCleanupBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        // Set execution time to 2:00 AM every day
        _executionTime = new TimeSpan(2, 0, 0);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Booking cleanup background service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var now = DateTime.Now;
                var nextRun = GetNextRunTime(now);
                var delayTime = nextRun - now;

                if (delayTime.TotalSeconds > 0)
                {
                    _logger.LogInformation($"Next cleanup scheduled for {nextRun:yyyy-MM-dd HH:mm:ss}");
                    await Task.Delay(delayTime, stoppingToken);
                }

                // Execute cleanup
                await CleanupExpiredBookings(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Booking cleanup background service is stopping");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in booking cleanup background service");
                // Wait 1 minute before retrying
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }

    private async Task CleanupExpiredBookings(CancellationToken stoppingToken)
    {
        try
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var cleanupService = scope.ServiceProvider.GetRequiredService<IBookingCleanupService>();
                var deletedCount = await cleanupService.DeleteExpiredBookingsAsync();
                _logger.LogInformation($"Booking cleanup executed successfully. Deleted {deletedCount} expired bookings");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during booking cleanup execution");
        }
    }

    private DateTime GetNextRunTime(DateTime now)
    {
        var nextRun = now.Date.Add(_executionTime);

        // If the time has already passed today, schedule for tomorrow
        if (nextRun <= now)
        {
            nextRun = nextRun.AddDays(1);
        }

        return nextRun;
    }
}
