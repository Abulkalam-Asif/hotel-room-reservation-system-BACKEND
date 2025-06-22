using backend.Models;

namespace backend.Repositories
{
    public interface IBookingRepository
    {
        Task<Booking> AddBookingAsync(Booking booking);
        Task<List<Booking>> GetBookingsByDateAsync(DateTime date);
        Task<int> GetBookingsCountByDateAsync(DateTime date);
        Task<List<Booking>> GetBookingsForRoomAsync(int roomId, DateTime checkIn, DateTime checkOut);
        Task AddBookingsAsync(IEnumerable<Booking> bookings);
    }
}
