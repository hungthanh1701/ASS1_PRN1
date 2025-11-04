using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Assignment_1.Data
{
    public sealed class DatabaseContextManager
    {
        private static DatabaseContextManager? _instance;
        private static readonly object _lock = new object();
        private readonly ApplicationDbContext _context;

        private DatabaseContextManager()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            _context = new ApplicationDbContext(optionsBuilder.Options);
        }

        public static DatabaseContextManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DatabaseContextManager();
                        }
                    }
                }
                return _instance;
            }
        }

        public ApplicationDbContext Context => _context;

        public async Task EnsureDatabaseCreatedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
        }

        public async Task MigrateAsync()
        {
            await _context.Database.MigrateAsync();
        }
    }
}
