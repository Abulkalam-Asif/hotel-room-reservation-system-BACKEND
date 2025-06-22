using backend.Models;
using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
  public class HotelRepository : IHotelRepository
  {
    private readonly HotelDbContext _context;
    public HotelRepository(HotelDbContext context)
    {
      _context = context;
    }
    public async Task<Hotel?> GetHotelAsync()
    {
      // Include rooms when fetching hotel
      return await _context.Hotels.Include(h => h.Rooms).FirstOrDefaultAsync();
    }
    public async Task<int> GetTotalRoomsAsync()
    {
      var hotel = await _context.Hotels.FirstOrDefaultAsync();
      return hotel?.TotalRooms ?? 0;
    }
  }
}
