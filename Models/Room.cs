using System.Text.Json.Serialization;

namespace HotelsBookingWebApi.Models;

public class Room
{
    public int Id { get; set; }
    public int HotelId { get; set; }
    public int RoomTypeId { get; set; }

    // Navigation properties
    public Hotel Hotel { get; set; } = null!;

    public RoomType RoomType { get; set; } = null!;

    [JsonIgnore]
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
