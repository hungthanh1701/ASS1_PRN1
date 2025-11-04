using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Assignment_1.Models;

namespace Assignment_1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Room entity to match existing database
            modelBuilder.Entity<Room>(entity =>
            {
                entity.ToTable("Rooms");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("RoomID");

                // Convert enums <-> Vietnamese strings stored in DB
                var roomTypeConverter = new ValueConverter<RoomType, string>(
                    v => MapRoomTypeToString(v),
                    v => MapRoomTypeFromString(v));

                var roomStatusConverter = new ValueConverter<RoomStatus, string>(
                    v => MapRoomStatusToString(v),
                    v => MapRoomStatusFromString(v));

                entity.Property(e => e.Type).HasColumnName("RoomType").HasConversion(roomTypeConverter);
                entity.Property(e => e.Status).HasColumnName("Status").HasConversion(roomStatusConverter);
                entity.Property(e => e.PricePerNight).HasColumnName("Price");

                // NotMapped properties
                entity.Ignore(e => e.RoomNumber);
                entity.Ignore(e => e.Description);
                entity.Ignore(e => e.Capacity);
                entity.Ignore(e => e.Amenities);
                entity.Ignore(e => e.CreatedDate);
                entity.Ignore(e => e.UpdatedDate);
            });

            // Configure Customer entity to match existing database
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customers");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("CustomerID");
                entity.Property(e => e.FullName).HasColumnName("Name");
                entity.Property(e => e.PhoneNumber).HasColumnName("PhoneNumber");
                entity.Property(e => e.Email).HasColumnName("Email");
                entity.Property(e => e.Address).HasColumnName("Address");
            });

            // Configure Booking entity to match existing database
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("Bookings");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("BookingID");
                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
                entity.Property(e => e.RoomId).HasColumnName("RoomID");
                entity.Property(e => e.CheckInDate).HasColumnName("CheckInDate");
                entity.Property(e => e.CheckOutDate).HasColumnName("CheckOutDate");
                entity.Property(e => e.TotalAmount).HasColumnName("TotalPrice");
                var bookingStatusConverter = new ValueConverter<BookingStatus, string>(
                    v => MapBookingStatusToString(v),
                    v => MapBookingStatusFromString(v));
                entity.Property(e => e.Status).HasColumnName("Status").HasConversion(bookingStatusConverter);
                entity.Property(e => e.IsDeleted).HasColumnName("IsDeleted");

                entity.HasOne(e => e.Customer)
                    .WithMany(c => c.Bookings)
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Room)
                    .WithMany(r => r.Bookings)
                    .HasForeignKey(e => e.RoomId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Staff entity to match existing database
            modelBuilder.Entity<Staff>(entity =>
            {
                entity.ToTable("Staff");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("StaffID");
                entity.Property(e => e.Name).HasColumnName("Name");

                var staffRoleConverter = new ValueConverter<StaffRole, string>(
                    v => MapStaffRoleToString(v),
                    v => MapStaffRoleFromString(v));

                entity.Property(e => e.Role).HasColumnName("Role").HasConversion(staffRoleConverter);
                entity.Property(e => e.PhoneNumber).HasColumnName("PhoneNumber");
                entity.Property(e => e.Email).HasColumnName("Email");
                entity.Property(e => e.IsActive).HasColumnName("IsActive");
            });

            // Configure AuditLog entity to match existing database
            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.ToTable("AuditLogs");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("LogID");
                entity.Property(e => e.TableName).HasColumnName("TableName");
                entity.Property(e => e.OperationType).HasColumnName("Operation");
                entity.Property(e => e.RecordId).HasColumnName("RecordID");
                entity.Property(e => e.OperationDate).HasColumnName("OperationDate");
                entity.Property(e => e.StaffId).HasColumnName("StaffID");
                entity.Property(e => e.Details).HasColumnName("Details");

                entity.HasOne(e => e.Staff)
                    .WithMany(s => s.AuditLogs)
                    .HasForeignKey(e => e.StaffId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // No seed data - using existing database
        }

        // Mapping helpers (avoid switch expressions inside expression trees)
        private static string MapRoomTypeToString(RoomType value)
        {
            switch (value)
            {
                case RoomType.Single: return "Phòng đơn";
                case RoomType.Double: return "Phòng đôi";
                case RoomType.Suite: return "Phòng suite";
                case RoomType.Deluxe: return "Phòng tiêu chuẩn";
                case RoomType.Presidential: return "Phòng cao cấp";
                default: return "Phòng đơn";
            }
        }

        private static RoomType MapRoomTypeFromString(string value)
        {
            switch (value)
            {
                case "Phòng đơn": return RoomType.Single;
                case "Phòng đôi": return RoomType.Double;
                case "Phòng suite": return RoomType.Suite;
                case "Phòng tiêu chuẩn": return RoomType.Deluxe;
                case "Phòng cao cấp": return RoomType.Presidential;
                default: return RoomType.Single;
            }
        }

        private static string MapRoomStatusToString(RoomStatus value)
        {
            switch (value)
            {
                case RoomStatus.Available: return "Rảnh";
                case RoomStatus.Occupied: return "Đã đặt";
                case RoomStatus.Maintenance: return "Bảo trì";
                case RoomStatus.Reserved: return "Đã giữ";
                default: return "Rảnh";
            }
        }

        private static RoomStatus MapRoomStatusFromString(string value)
        {
            switch (value)
            {
                case "Rảnh": return RoomStatus.Available;
                case "Đã đặt": return RoomStatus.Occupied;
                case "Bảo trì": return RoomStatus.Maintenance;
                case "Đã giữ": return RoomStatus.Reserved;
                default: return RoomStatus.Available;
            }
        }

        private static string MapStaffRoleToString(StaffRole value)
        {
            switch (value)
            {
                case StaffRole.Manager: return "Quản lý";
                case StaffRole.Receptionist: return "Lễ tân";
                case StaffRole.Housekeeping: return "Buồng phòng";
                case StaffRole.Maintenance: return "Bảo trì";
                case StaffRole.Admin: return "Quản trị";
                default: return "Lễ tân";
            }
        }

        private static StaffRole MapStaffRoleFromString(string value)
        {
            switch (value)
            {
                case "Quản lý": return StaffRole.Manager;
                case "Lễ tân": return StaffRole.Receptionist;
                case "Buồng phòng": return StaffRole.Housekeeping;
                case "Bảo trì": return StaffRole.Maintenance;
                case "Quản trị": return StaffRole.Admin;
                default: return StaffRole.Receptionist;
            }
        }

        private static string MapBookingStatusToString(BookingStatus value)
        {
            switch (value)
            {
                case BookingStatus.Pending: return "Chờ xác nhận";
                case BookingStatus.Confirmed: return "Đã xác nhận";
                case BookingStatus.CheckedIn: return "Đang ở";
                case BookingStatus.CheckedOut: return "Hoàn thành";
                case BookingStatus.Cancelled: return "Đã hủy";
                default: return "Chờ xác nhận";
            }
        }

        private static BookingStatus MapBookingStatusFromString(string value)
        {
            switch (value)
            {
                case "Chờ xác nhận": return BookingStatus.Pending;
                case "Đã xác nhận": return BookingStatus.Confirmed;
                case "Đang ở": return BookingStatus.CheckedIn;
                case "Hoàn thành": return BookingStatus.CheckedOut;
                case "Đã hủy": return BookingStatus.Cancelled;
                default: return BookingStatus.Pending;
            }
        }
    }
}