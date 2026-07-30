using System.ComponentModel.DataAnnotations;

namespace HotelsBookingWebApi.Models;

public class Booking
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public int RoomId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    // Navigation property
    public Room Room { get; set; } = null!;
}
