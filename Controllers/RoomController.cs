using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Repositories;
using backend.Services;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomController : ControllerBase
{
    private readonly IRoomRepository _roomRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IPricingService _pricingService;
    private readonly IHotelRepository _hotelRepository;

    public RoomController(IRoomRepository roomRepository, IBookingRepository bookingRepository, IPricingService pricingService, IHotelRepository hotelRepository)
    {
        _roomRepository = roomRepository;
        _bookingRepository = bookingRepository;
        _pricingService = pricingService;
        _hotelRepository = hotelRepository;
    }

    [HttpGet("available")]
    public async Task<ActionResult<List<object>>> GetAvailableRooms(
        [FromQuery] DateTime checkIn,
        [FromQuery] DateTime checkOut,
        [FromQuery] int adults = 1,
        [FromQuery] int kids = 0,
        [FromQuery] int rooms = 1)
    {
        var availableRooms = await _roomRepository.GetAvailableRoomsAsync(checkIn, checkOut);
        var totalRooms = await _roomRepository.GetTotalRoomsAsync();
        var bookedRooms = await _roomRepository.GetBookedRoomsCountAsync(checkIn);
        var result = new List<object>();
        foreach (var room in availableRooms)
        {
            var dynamicPrice = _pricingService.CalculateDynamicPrice(room.RackRate, totalRooms, bookedRooms);
            result.Add(new {
                room.Id,
                room.Title,
                Price = dynamicPrice,
                room.ImageUrl,
                room.Amenities
            });
        }
        return Ok(result);
    }

    [HttpGet("details")]
    public async Task<ActionResult<List<object>>> GetRoomDetails([FromQuery] List<int> ids)
    {
        if (ids == null || ids.Count == 0)
            return BadRequest("No room IDs provided.");
        var hotel = await _hotelRepository.GetHotelAsync();
        if (hotel == null) return NotFound();
        var totalRooms = await _roomRepository.GetTotalRoomsAsync();
        var bookedRooms = await _roomRepository.GetBookedRoomsCountAsync(DateTime.Now);
        var result = new List<object>();
        foreach (var id in ids)
        {
            var room = hotel.Rooms?.FirstOrDefault(r => r.Id == id);
            if (room != null)
            {
                var dynamicPrice = _pricingService.CalculateDynamicPrice(room.RackRate, totalRooms, bookedRooms);
                result.Add(new {
                    id = room.Id,
                    title = room.Title,
                    price = dynamicPrice,
                    imageUrl = room.ImageUrl,
                    amenities = room.Amenities
                });
            }
        }
        return Ok(result);
    }
}
