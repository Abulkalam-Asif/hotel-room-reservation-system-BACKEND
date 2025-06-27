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
  private readonly IRoomRepository _roomRepository;
  private readonly IConfiguration _config;

  public BookingController(IBookingRepository bookingRepository, IRoomRepository roomRepository, IConfiguration config)
  {
    _bookingRepository = bookingRepository;
    _roomRepository = roomRepository;
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

    // If any rooms failed, do not send email, return error response
    if (failedRooms.Count > 0)
    {
      return Ok(new
      {
        ReservationNumber = reservationNumber,
        Message = $"Some rooms could not be booked: {string.Join(", ", failedRooms)}.",
        SuccessfulRooms = successfulBookings.Select(b => b.RoomId).ToList(),
        FailedRooms = failedRooms
      });
    }

    // Send summary email only if all rooms booked
    try
    {
      var smtpHost = _config["Smtp:Host"];
      var smtpPort = int.Parse(_config["Smtp:Port"] ?? "587");
      var smtpUser = _config["Smtp:User"];
      var smtpPass = _config["Smtp:Pass"];
      var fromEmail = _config["Smtp:From"] ?? smtpUser;
      var toEmail = string.IsNullOrWhiteSpace(request.Email) ? smtpUser : request.Email;
      var fromAddr = string.IsNullOrWhiteSpace(fromEmail) ? smtpUser : fromEmail;

      // Ensure we have valid email addresses
      if (string.IsNullOrWhiteSpace(fromAddr) || string.IsNullOrWhiteSpace(toEmail))
      {
        return Ok(new
        {
          ReservationNumber = reservationNumber,
          Message = "Booking confirmed, but email configuration is invalid.",
          SuccessfulRooms = successfulBookings.Select(b => b.RoomId).ToList(),
          FailedRooms = failedRooms
        });
      }

      // Fetch room titles for summary
      var roomTitles = new List<string>();
      int idx = 1;
      foreach (var booking in successfulBookings)
      {
        var room = await _roomRepository.GetRoomByIdAsync(booking.RoomId);
        var title = room?.Title ?? "Room";
        // Extract type after last ' - ' if present
        var type = title.Contains(" - ") ? title.Substring(title.LastIndexOf(" - ") + 3) : title;
        roomTitles.Add($"Room {idx} Category: {type}");
        idx++;
      }
      var nights = Math.Max(1, (request.CheckOut - request.CheckIn).Days);
      var totalRooms = successfulBookings.Count;
      var totalCharge = request.TotalCharge;
      var avgNightlyRate = (totalRooms > 0 && nights > 0) ? (totalCharge / (totalRooms * nights)) : 0;

      var emailBody = $@"YOUR BOOKING SUMMARY
Check-in: {request.CheckIn:yyyy-MM-dd}
Check-out: {request.CheckOut:yyyy-MM-dd}
{string.Join("\r\n", roomTitles)}
Total number of adults: {request.Adults}
Avg. nightly rate: €{avgNightlyRate:F2}
Total charge of the stay: €{totalCharge:F2}
This is the amount that will be charged to your credit card.".Replace("\n", "\r\n");

      var mail = new MailMessage(fromAddr, toEmail)
      {
        Subject = $"Hotel Booking Confirmation #{reservationNumber}",
        Body = emailBody
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
      Message = "Booking confirmed! Confirmation email sent.",
      SuccessfulRooms = successfulBookings.Select(b => b.RoomId).ToList(),
      FailedRooms = failedRooms
    });
  }
}
