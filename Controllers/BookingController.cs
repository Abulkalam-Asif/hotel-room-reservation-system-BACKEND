using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Repositories;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
  private readonly IBookingRepository _bookingRepository;
  private readonly IConfiguration _config;

  public BookingController(IBookingRepository bookingRepository, IConfiguration config)
  {
    _bookingRepository = bookingRepository;
    _config = config;
  }

  [HttpPost]
  public async Task<IActionResult> CreateBooking([FromBody] BookingRequest request)
  {
    if (request == null || request.RoomIds == null || request.RoomIds.Count == 0)
      return BadRequest("No rooms selected.");

    var failedRooms = new List<int>();
    var successfulBookings = new List<Booking>();
    var reservationNumber = DateTime.UtcNow.Ticks.ToString().Substring(8); // Short unique number

    foreach (var roomId in request.RoomIds)
    {
      // Check if room is available
      var existing = await _bookingRepository.GetBookingsForRoomAsync(roomId, request.CheckIn, request.CheckOut);
      if (existing.Any())
      {
        failedRooms.Add(roomId);
        continue;
      }
      var booking = new Booking
      {
        RoomId = roomId,
        GuestFirstName = request.GuestFirstName,
        GuestLastName = request.GuestLastName,
        Email = request.Email,
        Phone = request.Phone,
        Country = request.Country,
        Street = request.Street,
        City = request.City,
        PostalCode = request.PostalCode,
        CardFirstName = request.CardFirstName,
        CardLastName = request.CardLastName,
        CardNumber = request.CardNumber,
        CardType = request.CardType,
        CardExpMonth = request.CardExpMonth,
        CardExpYear = request.CardExpYear,
        CheckIn = request.CheckIn,
        CheckOut = request.CheckOut,
        Adults = request.Adults,
        Kids = request.Kids,
        Rooms = 1,
        NightlyRate = request.NightlyRate,
        TotalCharge = request.TotalCharge,
        IsPaid = request.IsPaid,
        ReservationNumber = reservationNumber
      };
      successfulBookings.Add(booking);
    }

    if (successfulBookings.Count > 0)
      await _bookingRepository.AddBookingsAsync(successfulBookings);

    // Send summary email
    try
    {
      var smtpHost = _config["Smtp:Host"];
      var smtpPort = int.Parse(_config["Smtp:Port"] ?? "587");
      var smtpUser = _config["Smtp:User"];
      var smtpPass = _config["Smtp:Pass"];
      var fromEmail = _config["Smtp:From"] ?? smtpUser;
      var toEmail = string.IsNullOrWhiteSpace(request.Email) ? smtpUser : request.Email;
      var fromAddr = string.IsNullOrWhiteSpace(fromEmail) ? smtpUser : fromEmail;
      var mail = new MailMessage(fromAddr, toEmail)
      {
        Subject = $"Your Hotel Booking Confirmation #{reservationNumber}",
        Body = $"Dear {request.GuestFirstName},\n\nYour booking is confirmed! Reservation #: {reservationNumber}\nCheck-in: {request.CheckIn:yyyy-MM-dd}\nCheck-out: {request.CheckOut:yyyy-MM-dd}\n" +
               string.Join("\n", successfulBookings.Select(b => $"Room: {b.RoomId}")) +
               $"\nTotal: {request.TotalCharge:C}\n\nThank you for booking with us!" +
               (failedRooms.Count > 0 ? $"\n\nThe following rooms could not be booked (already reserved): {string.Join(", ", failedRooms)}" : "")
      };
      using var smtp = new SmtpClient(smtpHost, smtpPort)
      {
        Credentials = new NetworkCredential(smtpUser, smtpPass),
        EnableSsl = true
      };
      await smtp.SendMailAsync(mail);
    }
    catch (Exception ex)
    {
      return Ok(new
      {
        ReservationNumber = reservationNumber,
        Message = $"Booking confirmed, but failed to send email: {ex.Message}",
        SuccessfulRooms = successfulBookings.Select(b => b.RoomId).ToList(),
        FailedRooms = failedRooms
      });
    }

    return Ok(new
    {
      ReservationNumber = reservationNumber,
      Message = failedRooms.Count == 0 ? "Booking confirmed! Confirmation email sent." : $"Some rooms could not be booked: {string.Join(", ", failedRooms)}.",
      SuccessfulRooms = successfulBookings.Select(b => b.RoomId).ToList(),
      FailedRooms = failedRooms
    });
  }

  [HttpGet]
  public async Task<ActionResult<List<Booking>>> GetBookings()
  {
    // Return all bookings from DB
    var all = await _bookingRepository.GetBookingsByDateAsync(DateTime.UtcNow);
    return Ok(all);
  }
}
