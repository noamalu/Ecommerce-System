using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IPasswordHasher<object>, PasswordHasher<object>>();


// Configure logging
builder.Logging.ClearProviders(); // Clear all default logging providers
builder.Logging.AddFile("logs/app.log"); // Specify the log file path

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
