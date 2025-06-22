using backend.Models;

namespace backend.Repositories
{
    public interface IHotelRepository
    {
        Task<Hotel?> GetHotelAsync();
        Task<int> GetTotalRoomsAsync();
    }
}
