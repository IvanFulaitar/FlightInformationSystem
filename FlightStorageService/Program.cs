using FlightStorageService.Services;
using FlightStorageService.Repositories;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Реєструє бізнес-логіку рейсів на один HTTP-запит.
builder.Services.AddScoped<IFlightService, FlightService>();

// Реєструє доступ до сховища рейсів на один HTTP-запит.
builder.Services.AddScoped<IFlightRepository, FlightRepository>();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Підключає маршрути контролерів.
app.MapControllers();

app.Run();
