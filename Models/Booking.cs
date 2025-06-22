using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
  public class Booking
  {
    [Key]
    public int Id { get; set; }
    public int RoomId { get; set; }
    [ForeignKey("RoomId")]
    public Room? Room { get; set; }
    public string? GuestFirstName { get; set; }
    public string? GuestLastName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Country { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public string? CardFirstName { get; set; }
    public string? CardLastName { get; set; }
    public string? CardNumber { get; set; }
    public string? CardType { get; set; }
    public string? CardExpMonth { get; set; }
    public string? CardExpYear { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int Adults { get; set; }
    public int Kids { get; set; }
    public int Rooms { get; set; }
    public decimal NightlyRate { get; set; }
    public decimal TotalCharge { get; set; }
    public bool IsPaid { get; set; } = false;
    public string? ReservationNumber { get; set; }
  }
}