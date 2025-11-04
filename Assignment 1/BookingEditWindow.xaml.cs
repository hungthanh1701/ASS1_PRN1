using System;
using System.Linq;
using System.Windows;
using Assignment_1.Models;
namespace Assignment_1
{
    public partial class BookingEditWindow : Window
    {
        public Booking Booking { get; private set; }
        public BookingEditWindow(Booking booking, Customer[] customers, Room[] rooms)
        {
            InitializeComponent();
            Booking = booking;
            CustomerCombo.ItemsSource = customers;
            RoomCombo.ItemsSource = rooms;
            StatusCombo.ItemsSource = Enum.GetValues(typeof(BookingStatus)).Cast<BookingStatus>();
            CustomerCombo.SelectedItem = customers.FirstOrDefault(c => c.Id == booking.CustomerId);
            RoomCombo.SelectedItem = rooms.FirstOrDefault(r => r.Id == booking.RoomId);
            CheckInPicker.SelectedDate = booking.CheckInDate == default ? DateTime.Today : booking.CheckInDate;
            CheckOutPicker.SelectedDate = booking.CheckOutDate == default ? DateTime.Today.AddDays(1) : booking.CheckOutDate;
            StatusCombo.SelectedItem = booking.Status;
            TotalText.Text = booking.TotalAmount.ToString();
        }
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (CustomerCombo.SelectedItem == null || RoomCombo.SelectedItem == null)
            {
                MessageBox.Show("Please select customer and room.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!CheckInPicker.SelectedDate.HasValue || !CheckOutPicker.SelectedDate.HasValue || CheckInPicker.SelectedDate.Value >= CheckOutPicker.SelectedDate.Value)
            {
                MessageBox.Show("Check-in must be before check-out.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!decimal.TryParse(TotalText.Text, out var total))
            {
                MessageBox.Show("Total price must be a number.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
           
            if (total < 0)
            {
                MessageBox.Show("Total price cannot be negative.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
           
            if (total <= 0)
            {
                MessageBox.Show("Total price must be greater than 0.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Booking.CustomerId = ((Customer)CustomerCombo.SelectedItem).Id;
            Booking.RoomId = ((Room)RoomCombo.SelectedItem).Id;
            Booking.CheckInDate = CheckInPicker.SelectedDate!.Value;
            Booking.CheckOutDate = CheckOutPicker.SelectedDate!.Value;
            Booking.TotalAmount = (float)total;
            Booking.Status = (BookingStatus)StatusCombo.SelectedItem!;
            DialogResult = true;
        }
    }
}