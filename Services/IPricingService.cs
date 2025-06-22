using backend.Models;

namespace backend.Services
{
    public interface IPricingService
    {
        decimal CalculateDynamicPrice(decimal rackRate, int totalRooms, int bookedRooms);
    }
}
