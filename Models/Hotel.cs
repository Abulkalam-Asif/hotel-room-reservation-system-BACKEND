using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Hotel
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public int StarRating { get; set; }
        public string? MainImageUrl { get; set; }
        public int TotalRooms { get; set; } = 50;
        public List<Room>? Rooms { get; set; }
    }



}