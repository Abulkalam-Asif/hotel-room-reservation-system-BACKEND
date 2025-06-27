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
}
