namespace HotelsBookingWebApi.Services;

public interface IBookingCleanupService
{
    /// <summary>
    /// Delete all expired bookings 
    /// </summary>
    /// <returns>Number of deleted bookings</returns>
    Task<int> DeleteExpiredBookingsAsync();
}
