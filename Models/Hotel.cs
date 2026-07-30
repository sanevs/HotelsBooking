using System.Text.Json.Serialization;

namespace HotelsBookingWebApi.Models;

public class Hotel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Navigation property
    [JsonIgnore]
    public ICollection<Room> Rooms { get; set; } = new List<Room>();
}
