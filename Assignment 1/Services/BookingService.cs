using Assignment_1.Models;
using Assignment_1.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Assignment_1.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly ICustomerRepository _customerRepository;

        public BookingService(
            IBookingRepository bookingRepository,
            IRoomRepository roomRepository,
            ICustomerRepository customerRepository)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _bookingRepository.GetAllDetailedAsync();
        }

        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            return await _bookingRepository.GetByIdAsync(id);
        }

        public async Task<Booking> CreateBookingAsync(Booking booking)
        {
            if (!await ValidateBookingAsync(booking))
            {
                throw new ValidationException("Booking validation failed");
            }

            // Check if room is available
            if (!await IsRoomAvailableAsync(booking.RoomId, booking.CheckInDate, booking.CheckOutDate))
            {
                throw new InvalidOperationException("Room is not available for the selected dates");
            }

            return await _bookingRepository.AddAsync(booking);
        }

        public async Task<Booking> UpdateBookingAsync(Booking booking)
        {
            if (!await ValidateBookingAsync(booking))
            {
                throw new ValidationException("Booking validation failed");
            }

            // Check if room is available (excluding current booking)
            if (!await IsRoomAvailableAsync(booking.RoomId, booking.CheckInDate, booking.CheckOutDate, booking.Id))
            {
                throw new InvalidOperationException("Room is not available for the selected dates");
            }

            await _bookingRepository.UpdateAsync(booking);
            return booking;
        }

        public async Task DeleteBookingAsync(int id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking != null && !booking.CanBeCancelled)
            {
                throw new InvalidOperationException("Cannot delete booking that has been checked in or checked out");
            }

            await _bookingRepository.DeleteAsync(id);
        }

        public async Task<Booking> CancelBookingAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            if (booking == null)
            {
                throw new ArgumentException("Booking not found");
            }

            if (!booking.CanBeCancelled)
            {
                throw new InvalidOperationException("Booking cannot be cancelled");
            }

            booking.Status = BookingStatus.Cancelled;
            // No UpdatedDate in existing schema

            await _bookingRepository.UpdateAsync(booking);
            return booking;
        }

        public async Task<Booking> CheckInBookingAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            if (booking == null)
            {
                throw new ArgumentException("Booking not found");
            }

            if (booking.Status != BookingStatus.Confirmed)
            {
                throw new InvalidOperationException("Only confirmed bookings can be checked in");
            }

            booking.Status = BookingStatus.CheckedIn;
            // No UpdatedDate in existing schema

            // Update room status
            var room = await _roomRepository.GetByIdAsync(booking.RoomId);
            if (room != null)
            {
                room.Status = RoomStatus.Occupied;
                await _roomRepository.UpdateAsync(room);
            }

            await _bookingRepository.UpdateAsync(booking);
            return booking;
        }

        public async Task<Booking> CheckOutBookingAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            if (booking == null)
            {
                throw new ArgumentException("Booking not found");
            }

            if (booking.Status != BookingStatus.CheckedIn)
            {
                throw new InvalidOperationException("Only checked-in bookings can be checked out");
            }

            booking.Status = BookingStatus.CheckedOut;
            // No UpdatedDate in existing schema

            // Update room status
            var room = await _roomRepository.GetByIdAsync(booking.RoomId);
            if (room != null)
            {
                room.Status = RoomStatus.Available;
                await _roomRepository.UpdateAsync(room);
            }

            await _bookingRepository.UpdateAsync(booking);
            return booking;
        }

        public async Task<IEnumerable<Booking>> GetBookingsByCustomerIdAsync(int customerId)
        {
            return await _bookingRepository.GetByCustomerIdAsync(customerId);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByRoomIdAsync(int roomId)
        {
            return await _bookingRepository.GetByRoomIdAsync(roomId);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByStatusAsync(BookingStatus status)
        {
            return await _bookingRepository.GetByStatusAsync(status);
        }

        public async Task<IEnumerable<Booking>> GetActiveBookingsAsync()
        {
            return await _bookingRepository.GetActiveBookingsAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _bookingRepository.GetByDateRangeAsync(startDate, endDate);
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsDetailedAsync()
        {
            return await _bookingRepository.GetAllDetailedAsync();
        }

        public async Task<float> CalculateBookingTotalAsync(int roomId, DateTime checkIn, DateTime checkOut, int numberOfGuests)
        {
            var room = await _roomRepository.GetByIdAsync(roomId);
            if (room == null)
            {
                throw new ArgumentException("Room not found");
            }

            var numberOfNights = (checkOut - checkIn).Days;
            return (float)room.PricePerNight * numberOfNights;
        }

        public async Task<bool> ValidateBookingAsync(Booking booking)
        {
            if (booking == null)
                return false;

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(booking);

            bool isValid = Validator.TryValidateObject(booking, validationContext, validationResults, true);

            // Additional business validation
            if (booking.CheckInDate >= booking.CheckOutDate)
            {
                return false;
            }

            if (booking.CheckInDate < DateTime.Today)
            {
                return false;
            }

            // Check if customer exists
            var customer = await _customerRepository.GetByIdAsync(booking.CustomerId);
            if (customer == null)
            {
                return false;
            }

            // Check if room exists
            var room = await _roomRepository.GetByIdAsync(booking.RoomId);
            if (room == null)
            {
                return false;
            }

            // No capacity/NumberOfGuests in existing schema

            return await Task.FromResult(isValid);
        }

        public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut, int excludeBookingId = 0)
        {
            return await _bookingRepository.IsRoomAvailableAsync(roomId, checkIn, checkOut, excludeBookingId);
        }

        public async Task<float> CalculateTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            return await _bookingRepository.CalculateTotalRevenueAsync(startDate, endDate);
        }
    }
}
