using HotelsBookingWebApi.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelsBookingWebApi.Services;

public class BookingCleanupService : IBookingCleanupService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<BookingCleanupService> _logger;

    public BookingCleanupService(ApplicationDbContext context, ILogger<BookingCleanupService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<int> DeleteExpiredBookingsAsync()
    {
        try
        {
            var today = DateTime.Today;

            // Find all bookings with endDate <= today
            var expiredBookings = await _context.Bookings
                .Where(b => b.EndDate.Date <= today)
                .ToListAsync();

            if (expiredBookings.Count == 0)
            {
                _logger.LogInformation("No expired bookings found to delete");
                return 0;
            }

            // Delete expired bookings
            _context.Bookings.RemoveRange(expiredBookings);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Successfully deleted {expiredBookings.Count} expired bookings");
            return expiredBookings.Count;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting expired bookings");
            throw;
        }
    }
}
