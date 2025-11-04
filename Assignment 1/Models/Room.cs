using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment_1.Models
{
    public enum RoomStatus
    {
        Available = 0,
        Occupied = 1,
        Maintenance = 2,
        Reserved = 3
    }

    public enum RoomType
    {
        Single = 0,
        Double = 1,
        Suite = 2,
        Deluxe = 3,
        Presidential = 4
    }

    [Table("Rooms")]
    public class Room
    {
        [Key]
        [Column("RoomID")]
        public int Id { get; set; }

        [Required]
        [Column("RoomType")]
        public RoomType Type { get; set; }

        [Required]
        [Column("Status")]
        public RoomStatus Status { get; set; } = RoomStatus.Available;

        [Required]
        [Column("Price")]
        public double PricePerNight { get; set; }

        // Additional properties for UI (not in database)
        [NotMapped]
        public string RoomNumber => $"R{Id:D3}"; // Generate room number from ID

        [NotMapped]
        public string? Description { get; set; }

        [NotMapped]
        public int Capacity { get; set; } = 2; // Default capacity

        [NotMapped]
        public string? Amenities { get; set; }

        [NotMapped]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [NotMapped]
        public DateTime? UpdatedDate { get; set; }

        // Navigation properties
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
