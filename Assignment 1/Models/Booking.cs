using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment_1.Models
{
    public enum BookingStatus
    {
        Pending = 0,
        Confirmed = 1,
        CheckedIn = 2,
        CheckedOut = 3,
        Cancelled = 4
    }

    [Table("Bookings")]
    public class Booking
    {
        [Key]
        [Column("BookingID")]
        public int Id { get; set; }

        [Required]
        [Column("CustomerID")]
        public int CustomerId { get; set; }

        [Required]
        [Column("RoomID")]
        public int RoomId { get; set; }

        [Required]
        [Column("CheckInDate")]
        public DateTime CheckInDate { get; set; }

        [Required]
        [Column("CheckOutDate")]
        public DateTime CheckOutDate { get; set; }

        [Required]
        [Column("TotalPrice")]
        public double TotalAmount { get; set; }

        [Required]
        [Column("Status")]
        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        [Column("IsDeleted")]
        public bool? IsDeleted { get; set; }

        // Navigation properties
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; } = null!;

        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; } = null!;

        // Computed properties
        [NotMapped]
        public int NumberOfNights => (CheckOutDate - CheckInDate).Days;

        [NotMapped]
        public bool IsActive => Status == BookingStatus.Confirmed || Status == BookingStatus.CheckedIn;

        [NotMapped]
        public bool CanBeCancelled => Status == BookingStatus.Pending || Status == BookingStatus.Confirmed;
    }
}