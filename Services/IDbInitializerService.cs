namespace HotelsBookingWebApi.Services;

public interface IDbInitializerService
{
    Task SeedAsync();
    Task ResetAsync();
}
