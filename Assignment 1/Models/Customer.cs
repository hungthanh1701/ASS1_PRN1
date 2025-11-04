using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment_1.Models
{
    [Table("Customers")]
    public class Customer
    {
        [Key]
        [Column("CustomerID")]
        public int Id { get; set; }

        [Required]
        [Column("Name")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [Column("PhoneNumber")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [Column("Email")]
        public string Email { get; set; } = string.Empty;

        [Column("Address")]
        public string? Address { get; set; }

        // Navigation property
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}