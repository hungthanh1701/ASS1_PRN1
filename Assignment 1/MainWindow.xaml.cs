using System.Windows;
using Assignment_1.ViewModels;

namespace Assignment_1
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel; // ✅ Gắn ViewModel từ DI container
        }
    }
}
