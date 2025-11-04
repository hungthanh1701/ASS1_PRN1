using System;
using System.Linq;
using System.Windows;
using Assignment_1.Models;

namespace Assignment_1
{
    public partial class RoomEditWindow : Window
    {
        public Room Room { get; private set; }

        public RoomEditWindow(Room room)
        {
            InitializeComponent();
            Room = room;

            TypeCombo.ItemsSource = Enum.GetValues(typeof(RoomType)).Cast<RoomType>();
            StatusCombo.ItemsSource = Enum.GetValues(typeof(RoomStatus)).Cast<RoomStatus>();

            TypeCombo.SelectedItem = Room.Type;
            StatusCombo.SelectedItem = Room.Status;
            PriceText.Text = Room.PricePerNight.ToString();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (TypeCombo.SelectedItem == null || StatusCombo.SelectedItem == null)
            {
                MessageBox.Show("Please select Type and Status.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!float.TryParse(PriceText.Text, out var price) || price <= 0)
            {
                MessageBox.Show("Price must be a positive number.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Room.Type = (RoomType)TypeCombo.SelectedItem;
            Room.Status = (RoomStatus)StatusCombo.SelectedItem;
            Room.PricePerNight = price;

            DialogResult = true;
        }
    }
}


