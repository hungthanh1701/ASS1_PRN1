 ```csharp MainForm.cs
     using Microsoft.AspNetCore.Components.WebView.WindowsForms;

     public partial class MainForm : Form
     {
         public MainForm()
         {
             InitializeComponent();

             var blazorWebView = new BlazorWebView
             {
                 Dock = DockStyle.Fill,
                 HostPage = "wwwroot/index.html"
             };

             blazorWebView.Services = new ServiceCollection()
                 .AddBlazorWebView()
                 .BuildServiceProvider();

             Controls.Add(blazorWebView);
         }
     }
 ```