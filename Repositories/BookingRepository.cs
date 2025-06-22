using backend.Models;
using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly HotelDbContext _context;
        public BookingRepository(HotelDbContext context)
        {
            _context = context;
        }
        public async Task<Booking> AddBookingAsync(Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }
        public async Task AddBookingsAsync(IEnumerable<Booking> bookings)
        {
            _context.Bookings.AddRange(bookings);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Booking>> GetBookingsByDateAsync(DateTime date)
        {
            return await _context.Bookings.Where(b => b.CheckIn <= date && b.CheckOut >= date).ToListAsync();
        }
        public async Task<int> GetBookingsCountByDateAsync(DateTime date)
        {
            return await _context.Bookings.CountAsync(b => b.CheckIn <= date && b.CheckOut >= date);
        }
        public async Task<List<Booking>> GetBookingsForRoomAsync(int roomId, DateTime checkIn, DateTime checkOut)
        {
            return await _context.Bookings
                .Where(b => b.RoomId == roomId && b.CheckIn < checkOut && b.CheckOut > checkIn)
                .ToListAsync();
        }
    }
}
