using Assignment_1.Models;
using Assignment_1.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Assignment_1.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<IEnumerable<Room>> GetAllRoomsAsync()
        {
            return await _roomRepository.GetAllAsync();
        }

        public async Task<Room?> GetRoomByIdAsync(int id)
        {
            return await _roomRepository.GetByIdAsync(id);
        }

        public async Task<Room> CreateRoomAsync(Room room)
        {
            if (!await ValidateRoomAsync(room))
            {
                throw new ValidationException("Room validation failed");
            }

            if (!await IsRoomNumberUniqueAsync(room.RoomNumber))
            {
                throw new ValidationException("Room number already exists");
            }

            room.CreatedDate = DateTime.Now;
            room.UpdatedDate = null;

            return await _roomRepository.AddAsync(room);
        }

        public async Task<Room> UpdateRoomAsync(Room room)
        {
            if (!await ValidateRoomAsync(room))
            {
                throw new ValidationException("Room validation failed");
            }

            if (!await IsRoomNumberUniqueAsync(room.RoomNumber, room.Id))
            {
                throw new ValidationException("Room number already exists");
            }

            room.UpdatedDate = DateTime.Now;

            await _roomRepository.UpdateAsync(room);
            return room;
        }

        public async Task DeleteRoomAsync(int id)
        {
            // Check if room has active bookings
            var room = await _roomRepository.GetByIdAsync(id);
            if (room != null)
            {
                // You might want to add additional validation here
                // to prevent deletion of rooms with active bookings
            }

            await _roomRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Room>> SearchRoomsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllRoomsAsync();
            }

            return await _roomRepository.SearchByRoomNumberAsync(searchTerm);
        }

        public async Task<IEnumerable<Room>> GetRoomsByTypeAsync(RoomType roomType)
        {
            return await _roomRepository.GetByRoomTypeAsync(roomType);
        }

        public async Task<IEnumerable<Room>> GetRoomsByStatusAsync(RoomStatus status)
        {
            return await _roomRepository.GetByStatusAsync(status);
        }

        public async Task<IEnumerable<Room>> GetAvailableRoomsAsync()
        {
            return await _roomRepository.GetAvailableRoomsAsync();
        }

        public async Task<IEnumerable<Room>> GetAvailableRoomsForBookingAsync(DateTime checkIn, DateTime checkOut, int capacity)
        {
            if (checkIn >= checkOut)
            {
                throw new ArgumentException("Check-in date must be before check-out date");
            }

            if (capacity <= 0)
            {
                throw new ArgumentException("Capacity must be greater than 0");
            }

            return await _roomRepository.GetAvailableRoomsForBookingAsync(checkIn, checkOut, capacity);
        }

        public async Task<IEnumerable<Room>> GetRoomsByPriceRangeAsync(double minPrice, double maxPrice)
        {
            if (minPrice < 0 || maxPrice < 0 || minPrice > maxPrice)
            {
                throw new ArgumentException("Invalid price range");
            }

            return await _roomRepository.GetByPriceRangeAsync(minPrice, maxPrice);
        }

        public async Task<bool> ValidateRoomAsync(Room room)
        {
            if (room == null)
                return false;

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(room);

            bool isValid = Validator.TryValidateObject(room, validationContext, validationResults, true);

            return await Task.FromResult(isValid);
        }

        public async Task<bool> IsRoomNumberUniqueAsync(string roomNumber, int excludeId = 0)
        {
            return await _roomRepository.IsRoomNumberUniqueAsync(roomNumber, excludeId);
        }
    }
}
