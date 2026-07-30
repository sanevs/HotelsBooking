using HotelsBookingWebApi.Models;
using HotelsBookingWebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelsBookingWebApi.Controllers;

/// <summary>
/// Controller for managing hotels and searching for rooms
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HotelsController : ControllerBase
{
    private readonly IHotelsService _hotelsService;

    public HotelsController(IHotelsService hotelsService)
    {
        _hotelsService = hotelsService;
    }

    /// <summary>
    /// Search hotel by name
    /// </summary>
    /// <param name="name">Hotel name or part of the name</param>
    /// <returns>List of found hotels</returns>
    [HttpGet("search")]
    public async Task<ActionResult<Hotel>> GetHotelByName([FromQuery] string name)
    {
        try
        {
            var hotel = await _hotelsService.GetHotelByNameAsync(name);
            return Ok(hotel);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Get available rooms in the hotel for the specified period
    /// </summary>
    /// <param name="startDate">Booking start date</param>
    /// <param name="endDate">Booking end date</param>
    /// <param name="guestCount">Number of guests</param>
    /// <returns>List of available rooms matching the criteria</returns>
    [HttpGet("available-rooms")]
    public async Task<ActionResult<IEnumerable<Room>>> GetAvailableRooms(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        [FromQuery] int guestCount)
    {
        try
        {
            var availableRooms = await _hotelsService.GetAvailableRoomsAsync(startDate, endDate, guestCount);
            return Ok(availableRooms);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Book room at hotel name with date range and guest count
    /// </summary>
    /// <param name="bookingRequest"></param>
    /// <returns>Booking information</returns>
    [HttpPost("book-room")]
    public async Task<ActionResult<Booking>> BookRoom([FromBody] BookingRequest bookingRequest)
    {
        try
        {
            var booking = await _hotelsService.BookAsync(bookingRequest);
            return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, booking);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Get booking by ID
    /// </summary>
    /// <param name="id">Booking ID</param>
    /// <returns>Booking information</returns>
    [HttpGet("bookings/{id}")]
    public async Task<ActionResult<Booking>> GetBooking(Guid id)
    {
        var booking = await _hotelsService.GetBookingByIdAsync(id);

        if (booking == null)
        {
            return NotFound();
        }

        return Ok(booking);
    }

}
