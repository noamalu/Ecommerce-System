using System.Net;
using System.Net.Sockets;
using EcommerceAPI.Controllers;
using EcommerceAPI.initialize;
using MarketBackend.Domain.Payment;
using MarketBackend.Domain.Shipping;
using MarketBackend.Services;
using MarketBackend.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using WebSocketSharp.Server;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IPasswordHasher<object>, PasswordHasher<object>>();
builder.Services.AddSingleton<IShippingSystemFacade, ShippingSystemProxy>();
builder.Services.AddSingleton<IPaymentSystemFacade, PaymentSystemProxy>();
builder.Services.AddSingleton<IClientService, ClientService>();
builder.Services.AddSingleton<IMarketService, MarketService>();
builder.Services.AddSingleton<Configurate>(); // Register Configurate service

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:5173")
                           .AllowAnyHeader()
                           .AllowAnyMethod());
});

var app = builder.Build();

app.UseCors("AllowSpecificOrigin");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseSession();
app.UseAuthorization();
app.MapControllers();

// Initialize the WebSocket servers
var configurate = app.Services.GetRequiredService<Configurate>();
string port = configurate.Parse();
WebSocketServer alertServer = new WebSocketServer($"ws://{GetLocalIPAddress()}:{port}");
WebSocketServer logServer = new WebSocketServer($"ws://{GetLocalIPAddress()}:{port + 1}");

alertServer.Start();
logServer.Start();
builder.Services.AddSingleton(_ => alertServer);
builder.Services.AddSingleton(_ => logServer);

app.Run();

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
