using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace backend.Models
{
  public class Room
  {
    [Key]
    public int Id { get; set; }
    public int HotelId { get; set; }
    [ForeignKey("HotelId")]
    [JsonIgnore]
    public Hotel? Hotel { get; set; }
    public decimal Price { get; set; }
    public string? Title { get; set; }
    public decimal RackRate { get; set; } = 100;
    public string? ImageUrl { get; set; }
    public List<string>? Amenities { get; set; }
  }
}