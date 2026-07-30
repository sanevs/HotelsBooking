using HotelsBookingWebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelsBookingWebApi.Controllers;

/// <summary>
/// Controller for initializing and clearing the database. Available only in Development environment.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class InitializerController : ControllerBase
{
    private readonly IDbInitializerService _dbInitializerService;

    public InitializerController(IDbInitializerService dbInitializerService)
    {
        _dbInitializerService = dbInitializerService;
    }

    /// <summary>
    /// Initialize the database with test data.
    /// </summary>
    /// <returns>Message about successful initialization</returns>
    [HttpPost("seed")]
    public async Task<IActionResult> Seed()
    {
        try
        {
            await _dbInitializerService.SeedAsync();
            return Ok(new { message = "Database initialized successfully" });
        }
        catch(DuplicateWaitObjectException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Clear (reset) the database.
    /// </summary>
    /// <returns>Message about successful clearing</returns>
    [HttpDelete("reset")]
    public async Task<IActionResult> Reset()
    {
        try
        {
            await _dbInitializerService.ResetAsync();
            return Ok(new { message = "Database reset successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
