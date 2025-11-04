using System.Text.RegularExpressions;
using System.Windows;
using Assignment_1.Models;

namespace Assignment_1
{
    public partial class CustomerEditWindow : Window
    {
        public Customer Customer { get; private set; }

        public CustomerEditWindow(Customer customer)
        {
            InitializeComponent();
            Customer = customer;

            NameText.Text = customer.FullName;
            EmailText.Text = customer.Email;
            PhoneText.Text = customer.PhoneNumber;
            AddressText.Text = customer.Address;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameText.Text))
            {
                MessageBox.Show("Name is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(EmailText.Text) || !Regex.IsMatch(EmailText.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Invalid email.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(PhoneText.Text))
            {
                MessageBox.Show("Phone is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Customer.FullName = NameText.Text.Trim();
            Customer.Email = EmailText.Text.Trim();
            Customer.PhoneNumber = PhoneText.Text.Trim();
            Customer.Address = AddressText.Text.Trim();

            DialogResult = true;
        }
    }
}


