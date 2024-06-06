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

WebSocketServer notificationServer = new WebSocketServer($"ws://{GetLocalIPAddress()}:" + 7888);
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
builder.Logging.ClearProviders(); // Clear all default logging providers
// builder.Logging.AddFile("logs/app.log"); // Specify the log file path

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
