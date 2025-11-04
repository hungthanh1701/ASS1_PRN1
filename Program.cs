 ```csharp Program.cs
 using ElectronNET.API;

 public static class Program
 {
     [STAThread]
     public static void Main(string[] args)
     {
         var builder = WebApplication.CreateBuilder(args);

         // Add services to the container.
         builder.Services.AddRazorPages();

         var app = builder.Build();

         // Configure the HTTP request pipeline.
         if (!app.Environment.IsDevelopment())
         {
             app.UseExceptionHandler("/Error");
             app.UseHsts();
         }

         app.UseHttpsRedirection();
         app.UseStaticFiles();
         app.UseRouting();
         app.UseAuthorization();
         app.MapRazorPages();

         // Start Electron
         Task.Run(async () => await ElectronBootstrap());

         app.Run();
     }

     private static async Task ElectronBootstrap()
     {
             await Electron.WindowManager.CreateWindowAsync();
         }
     }
 ```