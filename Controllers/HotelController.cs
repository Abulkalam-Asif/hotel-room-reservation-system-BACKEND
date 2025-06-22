using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Repositories;
using System.Threading.Tasks;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HotelController : ControllerBase
{
  private readonly IHotelRepository _hotelRepository;

  public HotelController(IHotelRepository hotelRepository)
  {
    _hotelRepository = hotelRepository;
  }

  [HttpGet("info")]
  public async Task<ActionResult<Hotel>> GetHotelInfo()
  {
    var hotel = await _hotelRepository.GetHotelAsync();
    if (hotel == null) return NotFound();
    return Ok(hotel);
  }

  [HttpGet("rooms")]
  public async Task<ActionResult<List<Room>>> GetRooms()
  {
    var hotel = await _hotelRepository.GetHotelAsync();
    if (hotel == null) return NotFound();
    return Ok(hotel.Rooms);
  }

  [HttpGet("room/{id}")]
  public async Task<ActionResult<Room?>> GetRoom(int id)
  {
    var hotel = await _hotelRepository.GetHotelAsync();
    if (hotel == null) return NotFound();
    var room = hotel.Rooms?.FirstOrDefault(r => r.Id == id);
    if (room == null) return NotFound();
    return Ok(room);
  }
}
