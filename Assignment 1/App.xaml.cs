using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Assignment_1.Configuration;
using Assignment_1.Data;
using Assignment_1.Services;
using Assignment_1.ViewModels;
using System.Windows;
using System.IO;
using System.Threading.Tasks;

namespace Assignment_1
{
    public partial class App : Application
    {
        private IHost? _host;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Cấu hình host và DI container
            _host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    // Đăng ký các service và ViewModel
                    services.AddDatabaseServices(context.Configuration);
                    services.AddSingletonServices();

                    services.AddTransient<MainWindowViewModel>();
                    services.AddTransient<MainWindow>();
                })
                .Build();

            // Khởi tạo dữ liệu và hiển thị cửa sổ chính
            InitializeDatabase();
            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void InitializeDatabase()
        {
            try
            {
                using var scope = _host!.Services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Kiểm tra kết nối database
                if (!context.Database.CanConnect())
                {
                    MessageBox.Show("Không thể kết nối đến database.", "Database Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi database: {ex.Message}", "Database Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _host?.Dispose();
            base.OnExit(e);
        }
    }
}
