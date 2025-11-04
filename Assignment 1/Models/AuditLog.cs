using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment_1.Models
{
    public enum OperationType
    {
        Create = 0,
        Update = 1,
        Delete = 2,
        Login = 3,
        Logout = 4,
        View = 5
    }

    [Table("AuditLogs")]
    public class AuditLog
    {
        [Key]
        [Column("LogID")]
        public int Id { get; set; }

        [Required]
        [Column("TableName")]
        public string TableName { get; set; } = string.Empty;

        [Required]
        [Column("Operation")]
        public OperationType OperationType { get; set; }

        [Column("RecordID")]
        public int? RecordId { get; set; }

        [Required]
        [Column("OperationDate")]
        public DateTime OperationDate { get; set; } = DateTime.Now;

        [Column("StaffID")]
        public int? StaffId { get; set; }

        [Column("Details")]
        public string? Details { get; set; }

        // Navigation property
        [ForeignKey("StaffId")]
        public Staff? Staff { get; set; }

        // Computed property
        [NotMapped]
        public string OperationDisplayName => OperationType.ToString();
    }
}