using backend.Models;

namespace backend.Services
{
    public class PricingService : IPricingService
    {
        public decimal CalculateDynamicPrice(decimal rackRate, int totalRooms, int bookedRooms)
        {
            if (totalRooms == 0) return rackRate;
            var occupancy = (decimal)bookedRooms / totalRooms;
            if (occupancy < 0.3m)
                return rackRate * 0.8m; // 20% discount
            if (occupancy <= 0.7m)
                return rackRate; // full rate
            return rackRate * 1.2m; // 20% increase
        }
    }
}
