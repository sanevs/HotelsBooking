namespace HotelsBookingWebApi.Models
{
    public class BookingRequest
    {
        public string HotelName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int GuestCount { get; set; }
    }
}
