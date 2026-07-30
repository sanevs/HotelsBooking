using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HotelsBookingWebApi.Models;

public class RoomType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    [Range(1, 5, ErrorMessage = "Capacity must be between 1 and 5")]
    public int Capacity { get; set; }

    // Navigation property
    [JsonIgnore]
    public ICollection<Room> Rooms { get; set; } = new List<Room>();
}
