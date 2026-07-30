using HotelsBookingWebApi.Models;

namespace HotelsBookingWebApi.Services;

public interface IHotelsService
{
    Task<Hotel> GetHotelByNameAsync(string name);
    Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime startDate, DateTime endDate, int guestCount);
    Task<Booking> BookAsync(BookingRequest bookingRequest);
    Task<Booking?> GetBookingByIdAsync(Guid id);  
}
