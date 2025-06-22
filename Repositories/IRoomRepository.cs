using backend.Models;

namespace backend.Repositories
{
    public interface IRoomRepository
    {
        Task<List<Room>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut);
        Task<Room?> GetRoomByIdAsync(int id);
        Task<int> GetTotalRoomsAsync();
        Task<int> GetBookedRoomsCountAsync(DateTime date);
    }
}
