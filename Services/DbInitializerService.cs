using HotelsBookingWebApi.Data;
using HotelsBookingWebApi.Models;

namespace HotelsBookingWebApi.Services;

public class DbInitializerService : IDbInitializerService
{
    private readonly ApplicationDbContext _context;

    public DbInitializerService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task ResetAsync()
    {
        if (!_context.Hotels.Any())
        {
            throw new DuplicateWaitObjectException("Database is already empty");
        }

        // Delete data in reverse order of dependencies to avoid foreign key conflicts
        _context.Bookings.RemoveRange(_context.Bookings);
        _context.Rooms.RemoveRange(_context.Rooms);
        _context.RoomTypes.RemoveRange(_context.RoomTypes);
        _context.Hotels.RemoveRange(_context.Hotels);

        await _context.SaveChangesAsync();
    }

    public async Task SeedAsync()
    {
        // Check if data already exists
        if (_context.Hotels.Any())
        {
            throw new DuplicateWaitObjectException("Database is already seeded");
        }

        // Create Hotels
        var hotels = new List<Hotel>
        {
            new() { Id = 1, Name = "Grand Hotel" },
            new() { Id = 2, Name = "Seaside Resort" },
            new() { Id = 3, Name = "Mountain Lodge" }
        };
        _context.Hotels.AddRange(hotels);
        await _context.SaveChangesAsync();

        // Create RoomTypes
        var roomTypes = new List<RoomType>
        {
            new() { Id = 1, Name = "Single", Capacity = 1 },
            new() { Id = 2, Name = "Double", Capacity = 2 },
            new() { Id = 3, Name = "Deluxe", Capacity = 5 }
        };
        _context.RoomTypes.AddRange(roomTypes);
        await _context.SaveChangesAsync();

        // Create Rooms
        var rooms = new List<Room>
        {
            // Grand Hotel rooms
            new() { Id = 1, HotelId = 1, RoomTypeId = 1 },
            new() { Id = 2, HotelId = 1, RoomTypeId = 1 },
            new() { Id = 3, HotelId = 1, RoomTypeId = 2 }, 
            new() { Id = 4, HotelId = 1, RoomTypeId = 2 }, 
            new() { Id = 5, HotelId = 1, RoomTypeId = 2 }, 
            new() { Id = 6, HotelId = 1, RoomTypeId = 3 }, 

            // Seaside Resort rooms
            new() { Id = 7, HotelId = 2, RoomTypeId = 2 },
            new() { Id = 8, HotelId = 2, RoomTypeId = 2 },
            new() { Id = 9, HotelId = 2, RoomTypeId = 2 }, 
            new() { Id = 10, HotelId = 2, RoomTypeId = 3 }, 
            new() { Id = 11, HotelId = 2, RoomTypeId = 3 }, 
            new() { Id = 12, HotelId = 2, RoomTypeId = 3 }, 

            // Mountain Lodge rooms
            new() { Id = 13, HotelId = 3, RoomTypeId = 1 },
            new() { Id = 14, HotelId = 3, RoomTypeId = 1 },
            new() { Id = 15, HotelId = 3, RoomTypeId = 1 },
            new() { Id = 16, HotelId = 3, RoomTypeId = 2 },
            new() { Id = 17, HotelId = 3, RoomTypeId = 2 },
            new() { Id = 18, HotelId = 3, RoomTypeId = 2 },
        };

        _context.Rooms.AddRange(rooms);
        await _context.SaveChangesAsync();

        // Create Bookings
        var bookings = new List<Booking>
        {
            new()
            {
                RoomId = 1,
                StartDate = new DateTime(2026, 6, 1),
                EndDate = new DateTime(2026, 6, 5)
            },
            new()
            {
                RoomId = 1,
                StartDate = new DateTime(2026, 7, 10),
                EndDate = new DateTime(2026, 7, 14)
            },
            new()
            {
                RoomId = 2,
                StartDate = new DateTime(2026, 8, 3),
                EndDate = new DateTime(2026, 8, 8)
            },
            new()
            {
                RoomId = 2,
                StartDate = new DateTime(2026, 9, 20),
                EndDate = new DateTime(2026, 9, 25)
            },
            new()
            {
                RoomId = 3,
                StartDate = new DateTime(2026, 10, 1),
                EndDate = new DateTime(2026, 10, 7)
            },
            new()
            {
                RoomId = 3,
                StartDate = new DateTime(2026, 11, 15),
                EndDate = new DateTime(2026, 11, 20)
            },
            new()
            {
                RoomId = 4,
                StartDate = new DateTime(2027, 1, 5),
                EndDate = new DateTime(2027, 1, 12)
            },
            new()
            {
                RoomId = 4,
                StartDate = new DateTime(2027, 2, 20),
                EndDate = new DateTime(2027, 2, 28)
            },
            new()
            {
                RoomId = 5,
                StartDate = new DateTime(2027, 3, 10),
                EndDate = new DateTime(2027, 3, 15)
            },
            new()
            {
                RoomId = 5,
                StartDate = new DateTime(2027, 4, 1),
                EndDate = new DateTime(2027, 4, 6)
            },
            new()
            {
                RoomId = 6,
                StartDate = new DateTime(2027, 5, 22),
                EndDate = new DateTime(2027, 5, 30)
            },
            new()
            {
                RoomId = 6,
                StartDate = new DateTime(2027, 6, 12),
                EndDate = new DateTime(2027, 6, 18)
            },
            new()
            {
                RoomId = 7,
                StartDate = new DateTime(2027, 7, 5),
                EndDate = new DateTime(2027, 7, 13)
            },
            new()
            {
                RoomId = 7,
                StartDate = new DateTime(2027, 8, 1),
                EndDate = new DateTime(2027, 8, 9)
            },
            new()
            {
                RoomId = 8,
                StartDate = new DateTime(2027, 9, 17),
                EndDate = new DateTime(2027, 9, 24)
            },
            new()
            {
                RoomId = 8,
                StartDate = new DateTime(2027, 10, 3),
                EndDate = new DateTime(2027, 10, 10)
            },
            new()
            {
                RoomId = 9,
                StartDate = new DateTime(2026, 6, 1),
                EndDate = new DateTime(2026, 6, 5)
            },
            new()
            {
                RoomId = 9,
                StartDate = new DateTime(2026, 7, 10),
                EndDate = new DateTime(2026, 7, 14)
            },
            new()
            {
                RoomId = 10,
                StartDate = new DateTime(2026, 8, 3),
                EndDate = new DateTime(2026, 8, 8)
            },
            new()
            {
                RoomId = 10,
                StartDate = new DateTime(2026, 9, 20),
                EndDate = new DateTime(2026, 9, 25)
            },
            new()
            {
                RoomId = 11,
                StartDate = new DateTime(2026, 10, 1),
                EndDate = new DateTime(2026, 10, 7)
            },
            new()
            {
                RoomId = 11,
                StartDate = new DateTime(2026, 11, 15),
                EndDate = new DateTime(2026, 11, 20)
            },
            new()
            {
                RoomId = 12,
                StartDate = new DateTime(2027, 1, 5),
                EndDate = new DateTime(2027, 1, 12)
            },
            new()
            {
                RoomId = 12,
                StartDate = new DateTime(2027, 2, 20),
                EndDate = new DateTime(2027, 2, 28)
            },
            new()
            {
                RoomId = 13,
                StartDate = new DateTime(2027, 3, 10),
                EndDate = new DateTime(2027, 3, 15)
            },
            new()
            {
                RoomId = 13,
                StartDate = new DateTime(2027, 4, 1),
                EndDate = new DateTime(2027, 4, 6)
            },
            new()
            {
                RoomId = 14,
                StartDate = new DateTime(2027, 5, 22),
                EndDate = new DateTime(2027, 5, 30)
            },
            new()
            {
                RoomId = 14,
                StartDate = new DateTime(2027, 6, 12),
                EndDate = new DateTime(2027, 6, 18)
            },
            new()
            {
                RoomId = 15,
                StartDate = new DateTime(2027, 7, 5),
                EndDate = new DateTime(2027, 7, 13)
            },
            new()
            {
                RoomId = 15,
                StartDate = new DateTime(2027, 8, 1),
                EndDate = new DateTime(2027, 8, 9)
            },
            new()
            {
                RoomId = 16,
                StartDate = new DateTime(2027, 9, 17),
                EndDate = new DateTime(2027, 9, 24)
            },
            new()
            {
                RoomId = 16,
                StartDate = new DateTime(2027, 10, 3),
                EndDate = new DateTime(2027, 10, 10)
            },
            new()
            {
                RoomId = 17,
                StartDate = new DateTime(2027, 7, 5),
                EndDate = new DateTime(2027, 7, 13)
            },
            new()
            {
                RoomId = 17,
                StartDate = new DateTime(2027, 8, 1),
                EndDate = new DateTime(2027, 8, 9)
            },
            new()
            {
                RoomId = 18,
                StartDate = new DateTime(2027, 9, 17),
                EndDate = new DateTime(2027, 9, 24)
            },
            new()
            {
                RoomId = 18,
                StartDate = new DateTime(2027, 10, 3),
                EndDate = new DateTime(2027, 10, 10)
            }

        };

        _context.Bookings.AddRange(bookings);
        await _context.SaveChangesAsync();
    }
}
