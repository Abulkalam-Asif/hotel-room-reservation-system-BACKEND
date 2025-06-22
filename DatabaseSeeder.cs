using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend
{
  public static class DatabaseSeeder
  {
    public static void Seed(IServiceProvider serviceProvider)
    {
      using var scope = serviceProvider.CreateScope();
      var context = scope.ServiceProvider.GetRequiredService<HotelDbContext>();

      // Ensure database is created
      context.Database.Migrate();

      // Only seed if empty
      if (!context.Hotels.Any())
      {
        var hotel = new Hotel
        {
          Name = "Grand Azure Hotel",
          Address = "123 Azure Lane, Cloud City, 45678",
          StarRating = 4,
          MainImageUrl = "https://images.unsplash.com/photo-1549294413-26f195200c16?q=80&w=764&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
          TotalRooms = 40,
          Rooms = new List<Room>()
        };

        // Add 40 rooms
        for (int i = 1; i <= 40; i++)
        {
          hotel.Rooms.Add(new Room
          {
            Title = $"Room {i} - {(i % 2 == 0 ? "Deluxe" : "Standard")}",
            Price = 100 + (i % 3) * 20,
            RackRate = 120 + (i % 3) * 20,
            ImageUrl = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/698470557.jpg?k=84e4e2254ce1f11161b449421007eb9025818a5839cb323f996b553c39c89042&o=",
            Amenities = new List<string>
                        {
                            "Free internet access (public areas)",
                            "Non-smoking facility",
                            "Fitness center",
                            "Laundry",
                            "Lounge/bar",
                            "24h front desk",
                            "In-room safe",
                            "Business center"
                        }
          });
        }

        context.Hotels.Add(hotel);
        context.SaveChanges();
      }
    }
  }
}