using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment_1.Models
{
    public enum StaffRole
    {
        Admin = 0,
        Manager = 1,
        Receptionist = 2,
        Housekeeping = 3,
        Maintenance = 4
    }

    [Table("Staff")]
    public class Staff
    {
        [Key]
        [Column("StaffID")]
        public int Id { get; set; }

        [Required]
        [Column("Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column("Role")]
        public StaffRole Role { get; set; }

        [Required]
        [Column("PhoneNumber")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [Column("Email")]
        public string Email { get; set; } = string.Empty;

        [Column("IsActive")]
        public bool IsActive { get; set; } = true;

        // Navigation property
        public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
    }
}