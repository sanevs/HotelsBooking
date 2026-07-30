using HotelsBookingWebApi.Data;
using HotelsBookingWebApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add DbContext with conditional database provider
if (builder.Environment.IsDevelopment())
{
    // SQLite for development/testing
    var sqlitePath = Path.Combine(builder.Environment.ContentRootPath, "hotelsDb.sqlite");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite($"Data Source={sqlitePath}"));
}
else
{
    // SQL Server for production
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
        ?? "Server=.;Database=HotelsBookingDb;Trusted_Connection=true;Encrypt=false;";
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));
}

// Register services
builder.Services.AddScoped<IDbInitializerService, DbInitializerService>();
builder.Services.AddScoped<IHotelsService, HotelsService>();

builder.Services.AddControllers();

// Configure Swagger/OpenAPI
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "Hotels Booking API",
        Version = "v1",
        Description = "Booking API for managing hotel reservations",
        Contact = new OpenApiContact
        {
            Name = "Hotels Booking github",
            Url = new Uri("https://github.com/sanevs/HotelsBooking")
        }
    });

    // Add XML documentation
    var xmlFile = "HotelsBookingWebApi.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// Initialize database and seed data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();

    var dbInitializerService = scope.ServiceProvider.GetRequiredService<IDbInitializerService>();
    await dbInitializerService.ResetAsync();
    await dbInitializerService.SeedAsync();
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Hotels Booking API v1");
    options.RoutePrefix = string.Empty;
    options.DocumentTitle = "Hotels Booking API - Swagger UI";
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
