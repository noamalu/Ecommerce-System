using System.Net;
using System.Net.Sockets;
using MarketBackend.Domain.Payment;
using MarketBackend.Domain.Shipping;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using WebSocketSharp.Server;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add Distributed Memory Cache and Session services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Sets the timeout for the session
    options.Cookie.HttpOnly = true; // Makes the session cookie accessible only to the server
    options.Cookie.IsEssential = true; // Marks the session cookie as essential for the application
});

WebSocketServer notificationServer = new WebSocketServer(System.Net.IPAddress.Parse("127.0.0.1"), 4560);//new WebSocketServer($"ws://{GetLocalIPAddress()}:" + 7888);
// WebSocketServer logsServer = new WebSocketServer(System.Net.IPAddress.Parse("127.0.0.1"), 4560);
// logsServer.AddWebSocketService<logsService>("/logs");
notificationServer.Start();
// logsServer.Start();
builder.Services.AddSingleton(_ => notificationServer);
// builder.Services.AddSingleton(_ => logsServer);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IPasswordHasher<object>, PasswordHasher<object>>();
builder.Services.AddSingleton<IShippingSystemFacade, ShippingSystemProxy>();
builder.Services.AddSingleton<IPaymentSystemFacade, PaymentSystemProxy>();

static string GetLocalIPAddress()
{
    var host = Dns.GetHostEntry(Dns.GetHostName());
    foreach (var ip in host.AddressList)
    {
        if (ip.AddressFamily == AddressFamily.InterNetwork)
        {
            return ip.ToString();
        }
    }
    throw new Exception("No network adapters with an IPv4 address in the system!");
}
// Configure logging
//builder.Logging.ClearProviders(); // Clear all default logging providers
// builder.Logging.AddFile("logs/app.log"); // Specify the log file path

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:5173")  // Replace with your actual origin
                           .AllowAnyHeader()
                           .AllowAnyMethod());
});

var app = builder.Build();

app.UseCors("AllowSpecificOrigin");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseSession();

app.UseAuthorization();

app.MapControllers();

app.Run();
