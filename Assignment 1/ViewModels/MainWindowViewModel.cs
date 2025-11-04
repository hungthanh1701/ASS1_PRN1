using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using Assignment_1.Models;
using Assignment_1.Services;

namespace Assignment_1.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly IRoomService _roomService;
        private readonly ICustomerService _customerService;
        private readonly IBookingService _bookingService;
        private readonly IStaffService _staffService;
        private readonly IAuditLogService _auditLogService;

        // Room properties
        private ObservableCollection<Room> _rooms;
        private Room? _selectedRoom;
        private string _roomSearchTerm = string.Empty;
        private RoomType? _selectedRoomType;
        private RoomStatus? _selectedRoomStatus;
        private ObservableCollection<RoomType> _roomTypes;
        private ObservableCollection<RoomStatus> _roomStatuses;

        // Customer properties
        private ObservableCollection<Customer> _customers;
        private Customer? _selectedCustomer;
        private string _customerSearchTerm = string.Empty;

        // Booking properties
        private ObservableCollection<Booking> _bookings;
        private Booking? _selectedBooking;
        private string _bookingCustomerSearch = string.Empty;
        private string _bookingRoomSearch = string.Empty;
        private BookingStatus? _selectedBookingStatus;
        private ObservableCollection<BookingStatus> _bookingStatuses;

        // Staff properties
        private ObservableCollection<Staff> _staffMembers;
        private Staff? _selectedStaff;
        private string _staffSearchTerm = string.Empty;
        private StaffRole? _selectedStaffRole;
        private ObservableCollection<StaffRole> _staffRoles;

        // Audit Log properties
        private ObservableCollection<AuditLog> _auditLogs;
        private string _auditTableName = string.Empty;
        private OperationType? _selectedAuditOperation;
        private DateTime? _auditFromDate;
        private DateTime? _auditToDate;
        private ObservableCollection<OperationType> _auditOperations;

        public MainWindowViewModel(
            IRoomService roomService,
            ICustomerService customerService,
            IBookingService bookingService,
            IStaffService staffService,
            IAuditLogService auditLogService)
        {
            _roomService = roomService;
            _customerService = customerService;
            _bookingService = bookingService;
            _staffService = staffService;
            _auditLogService = auditLogService;

            // Initialize collections
            _rooms = new ObservableCollection<Room>();
            _customers = new ObservableCollection<Customer>();
            _bookings = new ObservableCollection<Booking>();
            _staffMembers = new ObservableCollection<Staff>();
            _auditLogs = new ObservableCollection<AuditLog>();
            _roomTypes = new ObservableCollection<RoomType>();
            _roomStatuses = new ObservableCollection<RoomStatus>();
            _bookingStatuses = new ObservableCollection<BookingStatus>();
            _staffRoles = new ObservableCollection<StaffRole>();
            _auditOperations = new ObservableCollection<OperationType>();

            // Initialize enums
            InitializeEnums();

            // Initialize commands
            InitializeCommands();

            // Load initial data
            _ = LoadInitialDataAsync();
        }

        // Room Properties
        public ObservableCollection<Room> Rooms
        {
            get => _rooms;
            set
            {
                _rooms = value;
                OnPropertyChanged();
            }
        }

        public Room? SelectedRoom
        {
            get => _selectedRoom;
            set
            {
                _selectedRoom = value;
                OnPropertyChanged();
                RaiseCanExecuteChangedForRoomCommands();
            }
        }

        public string RoomSearchTerm
        {
            get => _roomSearchTerm;
            set
            {
                _roomSearchTerm = value;
                OnPropertyChanged();
            }
        }

        public RoomType? SelectedRoomType
        {
            get => _selectedRoomType;
            set
            {
                _selectedRoomType = value;
                OnPropertyChanged();
            }
        }

        public RoomStatus? SelectedRoomStatus
        {
            get => _selectedRoomStatus;
            set
            {
                _selectedRoomStatus = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<RoomType> RoomTypes
        {
            get => _roomTypes;
            set
            {
                _roomTypes = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<RoomStatus> RoomStatuses
        {
            get => _roomStatuses;
            set
            {
                _roomStatuses = value;
                OnPropertyChanged();
            }
        }

        // Customer Properties
        public ObservableCollection<Customer> Customers
        {
            get => _customers;
            set
            {
                _customers = value;
                OnPropertyChanged();
            }
        }

        public Customer? SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged();
                RaiseCanExecuteChangedForCustomerCommands();
            }
        }

        public string CustomerSearchTerm
        {
            get => _customerSearchTerm;
            set
            {
                _customerSearchTerm = value;
                OnPropertyChanged();
            }
        }

        // Booking Properties
        public ObservableCollection<Booking> Bookings
        {
            get => _bookings;
            set
            {
                _bookings = value;
                OnPropertyChanged();
            }
        }

        public Booking? SelectedBooking
        {
            get => _selectedBooking;
            set
            {
                _selectedBooking = value;
                OnPropertyChanged();
                RaiseCanExecuteChangedForBookingCommands();
            }
        }

        public string BookingCustomerSearch
        {
            get => _bookingCustomerSearch;
            set
            {
                _bookingCustomerSearch = value;
                OnPropertyChanged();
            }
        }

        public string BookingRoomSearch
        {
            get => _bookingRoomSearch;
            set
            {
                _bookingRoomSearch = value;
                OnPropertyChanged();
            }
        }

        public BookingStatus? SelectedBookingStatus
        {
            get => _selectedBookingStatus;
            set
            {
                _selectedBookingStatus = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<BookingStatus> BookingStatuses
        {
            get => _bookingStatuses;
            set
            {
                _bookingStatuses = value;
                OnPropertyChanged();
            }
        }

        // Commands
        public ICommand SearchRoomsCommand { get; private set; }
        public ICommand AddRoomCommand { get; private set; }
        public ICommand EditRoomCommand { get; private set; }
        public ICommand DeleteRoomCommand { get; private set; }
        public ICommand RefreshRoomsCommand { get; private set; }

        public ICommand SearchCustomersCommand { get; private set; }
        public ICommand AddCustomerCommand { get; private set; }
        public ICommand EditCustomerCommand { get; private set; }
        public ICommand DeleteCustomerCommand { get; private set; }
        public ICommand RefreshCustomersCommand { get; private set; }

        public ICommand SearchBookingsCommand { get; private set; }
        public ICommand AddBookingCommand { get; private set; }
        public ICommand EditBookingCommand { get; private set; }
        public ICommand CancelBookingCommand { get; private set; }
        public ICommand CheckInBookingCommand { get; private set; }
        public ICommand CheckOutBookingCommand { get; private set; }
        public ICommand RefreshBookingsCommand { get; private set; }

        // Staff Properties
        public ObservableCollection<Staff> StaffMembers
        {
            get => _staffMembers;
            set
            {
                _staffMembers = value;
                OnPropertyChanged();
            }
        }

        public Staff? SelectedStaff
        {
            get => _selectedStaff;
            set
            {
                _selectedStaff = value;
                OnPropertyChanged();
                RaiseCanExecuteChangedForStaffCommands();
            }
        }

        public string StaffSearchTerm
        {
            get => _staffSearchTerm;
            set
            {
                _staffSearchTerm = value;
                OnPropertyChanged();
            }
        }

        public StaffRole? SelectedStaffRole
        {
            get => _selectedStaffRole;
            set
            {
                _selectedStaffRole = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<StaffRole> StaffRoles
        {
            get => _staffRoles;
            set
            {
                _staffRoles = value;
                OnPropertyChanged();
            }
        }

        // Audit Log Properties
        public ObservableCollection<AuditLog> AuditLogs
        {
            get => _auditLogs;
            set
            {
                _auditLogs = value;
                OnPropertyChanged();
            }
        }

        public string AuditTableName
        {
            get => _auditTableName;
            set
            {
                _auditTableName = value;
                OnPropertyChanged();
            }
        }

        public OperationType? SelectedAuditOperation
        {
            get => _selectedAuditOperation;
            set
            {
                _selectedAuditOperation = value;
                OnPropertyChanged();
            }
        }

        public DateTime? AuditFromDate
        {
            get => _auditFromDate;
            set
            {
                _auditFromDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime? AuditToDate
        {
            get => _auditToDate;
            set
            {
                _auditToDate = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<OperationType> AuditOperations
        {
            get => _auditOperations;
            set
            {
                _auditOperations = value;
                OnPropertyChanged();
            }
        }

        // Staff Commands
        public ICommand SearchStaffCommand { get; private set; }
        public ICommand AddStaffCommand { get; private set; }
        public ICommand EditStaffCommand { get; private set; }
        public ICommand DeactivateStaffCommand { get; private set; }
        public ICommand RefreshStaffCommand { get; private set; }

        // Audit Log Commands
        public ICommand FilterAuditLogsCommand { get; private set; }
        public ICommand RefreshAuditLogsCommand { get; private set; }
        public ICommand ExportAuditLogsCommand { get; private set; }

        private void InitializeEnums()
        {
            RoomTypes.Clear();
            foreach (RoomType roomType in Enum.GetValues<RoomType>())
            {
                RoomTypes.Add(roomType);
            }

            RoomStatuses.Clear();
            foreach (RoomStatus roomStatus in Enum.GetValues<RoomStatus>())
            {
                RoomStatuses.Add(roomStatus);
            }

            BookingStatuses.Clear();
            foreach (BookingStatus bookingStatus in Enum.GetValues<BookingStatus>())
            {
                BookingStatuses.Add(bookingStatus);
            }

            StaffRoles.Clear();
            foreach (StaffRole staffRole in Enum.GetValues<StaffRole>())
            {
                StaffRoles.Add(staffRole);
            }

            AuditOperations.Clear();
            foreach (OperationType operation in Enum.GetValues<OperationType>())
            {
                AuditOperations.Add(operation);
            }
        }

        private void InitializeCommands()
        {
            // Room commands
            SearchRoomsCommand = new RelayCommand(async () => await SearchRoomsAsync());
            AddRoomCommand = new RelayCommand(AddRoom);
            EditRoomCommand = new RelayCommand(EditRoom, () => SelectedRoom != null);
            DeleteRoomCommand = new RelayCommand(async () => await DeleteRoomAsync(), () => SelectedRoom != null);
            RefreshRoomsCommand = new RelayCommand(async () => await LoadRoomsAsync());

            // Customer commands
            SearchCustomersCommand = new RelayCommand(async () => await SearchCustomersAsync());
            AddCustomerCommand = new RelayCommand(AddCustomer);
            EditCustomerCommand = new RelayCommand(EditCustomer, () => SelectedCustomer != null);
            DeleteCustomerCommand = new RelayCommand(async () => await DeleteCustomerAsync(), () => SelectedCustomer != null);
            RefreshCustomersCommand = new RelayCommand(async () => await LoadCustomersAsync());

            // Booking commands
            SearchBookingsCommand = new RelayCommand(async () => await SearchBookingsAsync());
            AddBookingCommand = new RelayCommand(AddBooking);
            EditBookingCommand = new RelayCommand(EditBooking, () => SelectedBooking != null);
            CancelBookingCommand = new RelayCommand(async () => await CancelBookingAsync(), () => SelectedBooking?.CanBeCancelled == true);
            CheckInBookingCommand = new RelayCommand(async () => await CheckInBookingAsync(), () => SelectedBooking?.Status == BookingStatus.Confirmed);
            CheckOutBookingCommand = new RelayCommand(async () => await CheckOutBookingAsync(), () => SelectedBooking?.Status == BookingStatus.CheckedIn);
            RefreshBookingsCommand = new RelayCommand(async () => await LoadBookingsAsync());

            // Staff commands
            SearchStaffCommand = new RelayCommand(async () => await SearchStaffAsync());
            AddStaffCommand = new RelayCommand(AddStaff);
            EditStaffCommand = new RelayCommand(EditStaff, () => SelectedStaff != null);
            DeactivateStaffCommand = new RelayCommand(async () => await DeactivateStaffAsync(), () => SelectedStaff != null);
            RefreshStaffCommand = new RelayCommand(async () => await LoadStaffAsync());

            // Audit Log commands
            FilterAuditLogsCommand = new RelayCommand(async () => await FilterAuditLogsAsync());
            RefreshAuditLogsCommand = new RelayCommand(async () => await LoadAuditLogsAsync());
            ExportAuditLogsCommand = new RelayCommand(ExportAuditLogs);
        }

        private async Task LoadInitialDataAsync()
        {
            await LoadRoomsAsync();
            await LoadCustomersAsync();
            await LoadBookingsAsync();
            await LoadStaffAsync();
            await LoadAuditLogsAsync();
        }

        private void RaiseCanExecuteChangedForRoomCommands()
        {
            ((RelayCommand)EditRoomCommand).RaiseCanExecuteChanged();
            ((RelayCommand)DeleteRoomCommand).RaiseCanExecuteChanged();
        }

        private void RaiseCanExecuteChangedForCustomerCommands()
        {
            ((RelayCommand)EditCustomerCommand).RaiseCanExecuteChanged();
            ((RelayCommand)DeleteCustomerCommand).RaiseCanExecuteChanged();
        }

        private void RaiseCanExecuteChangedForBookingCommands()
        {
            ((RelayCommand)EditBookingCommand).RaiseCanExecuteChanged();
            ((RelayCommand)CancelBookingCommand).RaiseCanExecuteChanged();
            ((RelayCommand)CheckInBookingCommand).RaiseCanExecuteChanged();
            ((RelayCommand)CheckOutBookingCommand).RaiseCanExecuteChanged();
        }

        private void RaiseCanExecuteChangedForStaffCommands()
        {
            ((RelayCommand)EditStaffCommand).RaiseCanExecuteChanged();
            ((RelayCommand)DeactivateStaffCommand).RaiseCanExecuteChanged();
        }

        // Room Methods
        private async Task LoadRoomsAsync()
        {
            try
            {
                var rooms = await _roomService.GetAllRoomsAsync();
                Rooms.Clear();
                foreach (var room in rooms)
                {
                    Rooms.Add(room);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading rooms: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task SearchRoomsAsync()
        {
            try
            {
                IEnumerable<Room> results;

                if (!string.IsNullOrWhiteSpace(RoomSearchTerm))
                {
                    results = await _roomService.SearchRoomsAsync(RoomSearchTerm);
                }
                else if (SelectedRoomType.HasValue)
                {
                    results = await _roomService.GetRoomsByTypeAsync(SelectedRoomType.Value);
                }
                else if (SelectedRoomStatus.HasValue)
                {
                    results = await _roomService.GetRoomsByStatusAsync(SelectedRoomStatus.Value);
                }
                else
                {
                    results = await _roomService.GetAllRoomsAsync();
                }

                Rooms.Clear();
                foreach (var room in results)
                {
                    Rooms.Add(room);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching rooms: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void AddRoom()
        {
            var newRoom = new Room { Type = RoomType.Single, Status = RoomStatus.Available, PricePerNight = 100 };
            var wnd = new RoomEditWindow(newRoom) { Owner = Application.Current.MainWindow };
            if (wnd.ShowDialog() == true)
            {
                try
                {
                    await _roomService.CreateRoomAsync(newRoom);
                    await LoadRoomsAsync();
                    MessageBox.Show("Room added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error adding room: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void EditRoom()
        {
            if (SelectedRoom == null) return;

            var clone = new Room
            {
                Id = SelectedRoom.Id,
                Type = SelectedRoom.Type,
                Status = SelectedRoom.Status,
                PricePerNight = SelectedRoom.PricePerNight
            };

            var wnd = new RoomEditWindow(clone) { Owner = Application.Current.MainWindow };
            if (wnd.ShowDialog() == true)
            {
                try
                {
                    await _roomService.UpdateRoomAsync(clone);
                    await LoadRoomsAsync();
                    MessageBox.Show("Room updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating room: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async Task DeleteRoomAsync()
        {
            if (SelectedRoom != null)
            {
                var result = MessageBox.Show($"Are you sure you want to delete room '{SelectedRoom.RoomNumber}'?", 
                    "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        await _roomService.DeleteRoomAsync(SelectedRoom.Id);
                        await LoadRoomsAsync();
                        MessageBox.Show("Room deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting room: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        // Customer Methods
        private async Task LoadCustomersAsync()
        {
            try
            {
                var customers = await _customerService.GetAllCustomersAsync();
                Customers.Clear();
                foreach (var customer in customers)
                {
                    Customers.Add(customer);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading customers: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task SearchCustomersAsync()
        {
            try
            {
                var results = await _customerService.SearchCustomersAsync(CustomerSearchTerm);
                Customers.Clear();
                foreach (var customer in results)
                {
                    Customers.Add(customer);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching customers: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void AddCustomer()
        {
            var cust = new Customer();
            var wnd = new CustomerEditWindow(cust) { Owner = Application.Current.MainWindow };
            if (wnd.ShowDialog() == true)
            {
                try
                {
                    await _customerService.CreateCustomerAsync(cust);
                    await LoadCustomersAsync();
                    MessageBox.Show("Customer added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error adding customer: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void EditCustomer()
        {
            if (SelectedCustomer == null) return;
            var clone = new Customer
            {
                Id = SelectedCustomer.Id,
                FullName = SelectedCustomer.FullName,
                Email = SelectedCustomer.Email,
                PhoneNumber = SelectedCustomer.PhoneNumber,
                Address = SelectedCustomer.Address
            };
            var wnd = new CustomerEditWindow(clone) { Owner = Application.Current.MainWindow };
            if (wnd.ShowDialog() == true)
            {
                try
                {
                    await _customerService.UpdateCustomerAsync(clone);
                    await LoadCustomersAsync();
                    MessageBox.Show("Customer updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating customer: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async Task DeleteCustomerAsync()
        {
            if (SelectedCustomer != null)
            {
                var result = MessageBox.Show($"Are you sure you want to delete customer '{SelectedCustomer.FullName}'?", 
                    "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        await _customerService.DeleteCustomerAsync(SelectedCustomer.Id);
                        await LoadCustomersAsync();
                        MessageBox.Show("Customer deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting customer: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        // Booking Methods
        private async Task LoadBookingsAsync()
        {
            try
            {
                var bookings = await _bookingService.GetAllBookingsAsync();
                Bookings.Clear();
                foreach (var booking in bookings)
                {
                    Bookings.Add(booking);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading bookings: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task SearchBookingsAsync()
        {
            try
            {
                IEnumerable<Booking> results;

                if (!string.IsNullOrWhiteSpace(BookingCustomerSearch))
                {
                    // Search by customer name
                    var customers = await _customerService.SearchCustomersAsync(BookingCustomerSearch);
                    var customerIds = customers.Select(c => c.Id);
                    results = (await _bookingService.GetAllBookingsAsync()).Where(b => customerIds.Contains(b.CustomerId));
                }
                else if (!string.IsNullOrWhiteSpace(BookingRoomSearch))
                {
                    // Search by room number
                    var rooms = await _roomService.SearchRoomsAsync(BookingRoomSearch);
                    var roomIds = rooms.Select(r => r.Id);
                    results = (await _bookingService.GetAllBookingsAsync()).Where(b => roomIds.Contains(b.RoomId));
                }
                else if (SelectedBookingStatus.HasValue)
                {
                    results = await _bookingService.GetBookingsByStatusAsync(SelectedBookingStatus.Value);
                }
                else
                {
                    results = await _bookingService.GetAllBookingsAsync();
                }

                Bookings.Clear();
                foreach (var booking in results)
                {
                    Bookings.Add(booking);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching bookings: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void AddBooking()
        {
            var customers = (await _customerService.GetAllCustomersAsync()).ToArray();
            var rooms = (await _roomService.GetAllRoomsAsync()).ToArray();
            var booking = new Booking { Status = BookingStatus.Confirmed };
            var wnd = new BookingEditWindow(booking, customers, rooms) { Owner = Application.Current.MainWindow };
            if (wnd.ShowDialog() == true)
            {
                try
                {
                    await _bookingService.CreateBookingAsync(booking);
                    await LoadBookingsAsync();
                    await LoadRoomsAsync();
                    MessageBox.Show("Booking created successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error creating booking: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void EditBooking()
        {
            if (SelectedBooking == null) return;
            var customers = (await _customerService.GetAllCustomersAsync()).ToArray();
            var rooms = (await _roomService.GetAllRoomsAsync()).ToArray();
            var clone = new Booking
            {
                Id = SelectedBooking.Id,
                CustomerId = SelectedBooking.CustomerId,
                RoomId = SelectedBooking.RoomId,
                CheckInDate = SelectedBooking.CheckInDate,
                CheckOutDate = SelectedBooking.CheckOutDate,
                Status = SelectedBooking.Status,
                TotalAmount = SelectedBooking.TotalAmount
            };
            var wnd = new BookingEditWindow(clone, customers, rooms) { Owner = Application.Current.MainWindow };
            if (wnd.ShowDialog() == true)
            {
                try
                {
                    await _bookingService.UpdateBookingAsync(clone);
                    await LoadBookingsAsync();
                    await LoadRoomsAsync();
                    MessageBox.Show("Booking updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating booking: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async Task CancelBookingAsync()
        {
            if (SelectedBooking != null)
            {
                var result = MessageBox.Show($"Are you sure you want to cancel booking {SelectedBooking.Id}?", 
                    "Confirm Cancel", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        await _bookingService.CancelBookingAsync(SelectedBooking.Id);
                        await LoadBookingsAsync();
                        MessageBox.Show("Booking cancelled successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error cancelling booking: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private async Task CheckInBookingAsync()
        {
            if (SelectedBooking != null)
            {
                try
                {
                    await _bookingService.CheckInBookingAsync(SelectedBooking.Id);
                    await LoadBookingsAsync();
                    await LoadRoomsAsync(); // Refresh room status
                    MessageBox.Show("Check-in completed successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error checking in booking: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async Task CheckOutBookingAsync()
        {
            if (SelectedBooking != null)
            {
                try
                {
                    await _bookingService.CheckOutBookingAsync(SelectedBooking.Id);
                    await LoadBookingsAsync();
                    await LoadRoomsAsync(); // Refresh room status
                    MessageBox.Show("Check-out completed successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error checking out booking: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Staff Methods
        private async Task LoadStaffAsync()
        {
            try
            {
                var staff = await _staffService.GetAllStaffAsync();
                StaffMembers.Clear();
                foreach (var member in staff)
                {
                    StaffMembers.Add(member);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading staff: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task SearchStaffAsync()
        {
            try
            {
                IEnumerable<Staff> results;

                if (!string.IsNullOrWhiteSpace(StaffSearchTerm))
                {
                    results = await _staffService.SearchStaffAsync(StaffSearchTerm);
                }
                else if (SelectedStaffRole.HasValue)
                {
                    results = await _staffService.GetStaffByRoleAsync(SelectedStaffRole.Value);
                }
                else
                {
                    results = await _staffService.GetAllStaffAsync();
                }

                StaffMembers.Clear();
                foreach (var member in results)
                {
                    StaffMembers.Add(member);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching staff: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddStaff()
        {
            MessageBox.Show("Staff management window would open here", "Add Staff", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void EditStaff()
        {
            if (SelectedStaff != null)
            {
                MessageBox.Show($"Edit staff {SelectedStaff.Name}", "Edit Staff", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private async Task DeactivateStaffAsync()
        {
            if (SelectedStaff != null)
            {
                var result = MessageBox.Show($"Are you sure you want to deactivate staff '{SelectedStaff.Name}'?", 
                    "Confirm Deactivate", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        await _staffService.DeleteStaffAsync(SelectedStaff.Id);
                        await LoadStaffAsync();
                        MessageBox.Show("Staff deactivated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deactivating staff: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        // Audit Log Methods
        private async Task LoadAuditLogsAsync()
        {
            try
            {
                var logs = await _auditLogService.GetRecentAuditLogsAsync(100);
                AuditLogs.Clear();
                foreach (var log in logs)
                {
                    AuditLogs.Add(log);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading audit logs: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task FilterAuditLogsAsync()
        {
            try
            {
                IEnumerable<AuditLog> results;

                if (!string.IsNullOrWhiteSpace(AuditTableName))
                {
                    results = await _auditLogService.GetAuditLogsByTableNameAsync(AuditTableName);
                }
                else if (SelectedAuditOperation.HasValue)
                {
                    results = await _auditLogService.GetAuditLogsByOperationAsync(SelectedAuditOperation.Value);
                }
                else if (AuditFromDate.HasValue && AuditToDate.HasValue)
                {
                    results = await _auditLogService.GetAuditLogsByDateRangeAsync(AuditFromDate.Value, AuditToDate.Value);
                }
                else
                {
                    results = await _auditLogService.GetRecentAuditLogsAsync(100);
                }

                AuditLogs.Clear();
                foreach (var log in results)
                {
                    AuditLogs.Add(log);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error filtering audit logs: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExportAuditLogs()
        {
            MessageBox.Show("Export audit logs functionality would be implemented here", "Export Logs", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute?.Invoke() ?? true;
        }

        public void Execute(object? parameter)
        {
            _execute();
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
