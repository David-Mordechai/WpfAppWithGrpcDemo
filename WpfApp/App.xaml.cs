using System.Net;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using WpfApp.GrpcServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace WpfApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    protected override void OnStartup(StartupEventArgs eventArgs)
    {
        var builder = WebApplication.CreateBuilder();
        
        ConfigureServices(builder.Services, builder);
        
        builder.Services.AddGrpc();
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.Listen(IPAddress.Any, 5151, listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http2;
            });
        });

        var app = builder.Build();

        app.MapGrpcService<CommandsService>();
        app.MapGrpcService<ReportsService>();
        
        app.Start();

        var mainWindow = app.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    private static void ConfigureServices(IServiceCollection serviceCollection, WebApplicationBuilder builder)
    {
        var settings = builder.Configuration.GetSection(nameof(Settings)).Get<Settings>();
        serviceCollection.AddSingleton(settings);
        serviceCollection.AddTransient(typeof(MainWindow));
    }
}