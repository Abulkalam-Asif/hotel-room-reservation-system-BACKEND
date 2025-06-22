using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data
{
  public class HotelDbContext : DbContext
  {
    public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options) { }

    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      // Seed Hotels
      modelBuilder.Entity<Hotel>().HasData(
          new Hotel { Id = 1, Name = "Sample Hotel", Address = "123 Main St", StarRating = 4, MainImageUrl = "hotel1.jpg", TotalRooms = 2 }
      );

      // Seed Rooms
      modelBuilder.Entity<Room>().HasData(
          new Room { Id = 1, HotelId = 1, Title = "Deluxe Room", Price = 120, RackRate = 150, ImageUrl = "room1.jpg" },
          new Room { Id = 2, HotelId = 1, Title = "Suite", Price = 200, RackRate = 250, ImageUrl = "room2.jpg" }
      );

      // Seed Bookings
      modelBuilder.Entity<Booking>().HasData(
          new Booking
          {
            Id = 1,
            RoomId = 1,
            GuestFirstName = "John",
            GuestLastName = "Doe",
            Email = "john@example.com",
            Phone = "1234567890",
            Country = "USA",
            Street = "456 Elm St",
            City = "Metropolis",
            PostalCode = "12345",
            CardFirstName = "John",
            CardLastName = "Doe",
            CardNumber = "4111111111111111",
            CardType = "Visa",
            CardExpMonth = "12",
            CardExpYear = "2025",
            CheckIn = DateTime.Today,
            CheckOut = DateTime.Today.AddDays(2),
            Adults = 2,
            Kids = 0,
            Rooms = 1,
            NightlyRate = 120,
            TotalCharge = 240,
            IsPaid = true,
            ReservationNumber = "ABC123"
          }
      );

      // Set decimal precision for currency fields
      modelBuilder.Entity<Room>()
          .Property(r => r.Price)
          .HasColumnType("decimal(18,2)");
      modelBuilder.Entity<Room>()
          .Property(r => r.RackRate)
          .HasColumnType("decimal(18,2)");
      modelBuilder.Entity<Booking>()
          .Property(b => b.NightlyRate)
          .HasColumnType("decimal(18,2)");
      modelBuilder.Entity<Booking>()
          .Property(b => b.TotalCharge)
          .HasColumnType("decimal(18,2)");
    }
  }
}
