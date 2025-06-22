using backend.Models;
using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
  public class RoomRepository : IRoomRepository
  {
    private readonly HotelDbContext _context;
    public RoomRepository(HotelDbContext context)
    {
      _context = context;
    }
    public async Task<List<Room>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut)
    {
      // Get all rooms
      var allRooms = await _context.Rooms.ToListAsync();
      // Get all bookings that overlap with the requested dates
      var bookedRoomIds = await _context.Bookings
          .Where(b => b.CheckIn < checkOut && b.CheckOut > checkIn)
          .Select(b => b.RoomId)
          .ToListAsync();
      // Return rooms that are not booked
      return allRooms.Where(r => !bookedRoomIds.Contains(r.Id)).ToList();
    }
    public async Task<Room?> GetRoomByIdAsync(int id)
    {
      return await _context.Rooms.FindAsync(id);
    }
    public async Task<int> GetTotalRoomsAsync()
    {
      return await _context.Rooms.CountAsync();
    }
    public async Task<int> GetBookedRoomsCountAsync(DateTime date)
    {
      return await _context.Bookings.CountAsync(b => b.CheckIn <= date && b.CheckOut > date);
    }
  }
}
