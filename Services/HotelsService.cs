using HotelsBookingWebApi.Data;
using HotelsBookingWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelsBookingWebApi.Services;

public class HotelsService : IHotelsService
{
    private const string _notFound = "Hotel not found";

    private readonly ApplicationDbContext _context;

    public HotelsService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Hotel> GetHotelByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Hotel name cannot be empty", nameof(name));
        }

        if (!_context.Hotels.Any())
        {
            throw new KeyNotFoundException(_notFound);
        }

        var hotel = await _context.Hotels
            .Where(h => h.Name.Contains(name))
            .FirstOrDefaultAsync();

        return hotel ?? throw new KeyNotFoundException(_notFound);
    }

    public async Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime startDate, DateTime endDate, int guestCount)
    {
        if (startDate >= endDate)
        {
            throw new ArgumentException("Start date must be before end date");
        }

        if (guestCount <= 0)
        {
            throw new ArgumentException("Number of guests must be greater than zero");
        }
        
        if (startDate < DateTime.Today)
        {
            throw new ArgumentException("Start date must be a future date");
        }

        if (endDate < DateTime.Today)
        {
            throw new ArgumentException("End date must be a future date");
        }

        var occupiedBookings = await _context.Bookings 
            .Where(b => (startDate > b.StartDate && startDate < b.EndDate) 
                || (endDate > b.StartDate && endDate < b.EndDate)
                || (startDate < b.StartDate && endDate > b.EndDate))
            .ToListAsync();

        List<Room> availableRooms = await _context.Rooms
            .Include(r => r.RoomType)
            .Include(r => r.Hotel)
            .Where(r => r.RoomType.Capacity >= guestCount)
            .AsAsyncEnumerable()
            .Where(r => !occupiedBookings.Any(b => b.RoomId == r.Id))
            .ToListAsync();

        return availableRooms.Count != 0 
            ? availableRooms 
            : throw new KeyNotFoundException("No available rooms for the specified period and guest count");
    }

    public async Task<Booking> BookAsync(BookingRequest bookingRequest)
    {
        var hotel = await GetHotelByNameAsync(bookingRequest.HotelName);
        
        var availableRooms = await GetAvailableRoomsAsync(bookingRequest.StartDate, bookingRequest.EndDate, bookingRequest.GuestCount);

        var compatibleRoom = availableRooms.Where(r => r.HotelId == hotel.Id
            && r.RoomType.Capacity >= bookingRequest.GuestCount)
            .OrderBy(r => r.RoomType.Capacity)
            .FirstOrDefault();
        if(compatibleRoom is null)
        {
            throw new KeyNotFoundException("No available rooms in the specified hotel for the given period and guest count");
        }

        var booking = new Booking
        {
            RoomId = compatibleRoom.Id,
            StartDate = bookingRequest.StartDate,
            EndDate = bookingRequest.EndDate
        };

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        return booking;
    }

    public async Task<Booking?> GetBookingByIdAsync(Guid id)
    {
        return await _context.Bookings
            .Include(b => b.Room)
            .ThenInclude(r => r.Hotel)
            .Include(b => b.Room)
            .ThenInclude(r => r.RoomType)
            .FirstOrDefaultAsync(b => b.Id == id);
    }
}
