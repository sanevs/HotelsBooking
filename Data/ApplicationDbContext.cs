using HotelsBookingWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelsBookingWebApi.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<RoomType> RoomTypes { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Hotel
        modelBuilder.Entity<Hotel>()
            .HasKey(h => h.Id);

        modelBuilder.Entity<Hotel>()
            .Property(h => h.Name)
            .IsRequired()
            .HasMaxLength(255);

        // Configure RoomType
        modelBuilder.Entity<RoomType>()
            .HasKey(rt => rt.Id);

        modelBuilder.Entity<RoomType>()
            .Property(rt => rt.Name)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<RoomType>()
            .Property(rt => rt.Capacity);
        // Add check constraint for Capacity (1-5)
        modelBuilder.Entity<RoomType>()
            .ToTable(t => t.HasCheckConstraint("CK_RoomType_Capacity", "Capacity >= 1 AND Capacity <= 5"));

        // Configure Room
        modelBuilder.Entity<Room>()
            .HasKey(r => r.Id);

        modelBuilder.Entity<Room>()
            .HasOne(r => r.Hotel)
            .WithMany(h => h.Rooms)
            .HasForeignKey(r => r.HotelId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Room>()
            .HasOne(r => r.RoomType)
            .WithMany(rt => rt.Rooms)
            .HasForeignKey(r => r.RoomTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure Booking
        modelBuilder.Entity<Booking>()
            .HasKey(b => b.Id);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Room)
            .WithMany(r => r.Bookings)
            .HasForeignKey(b => b.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
