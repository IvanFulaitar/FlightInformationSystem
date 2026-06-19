using FlightStorageService.Services;
using FlightStorageService.Repositories;
using FlightStorageService.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Реєструє бізнес-логіку рейсів на один HTTP-запит.
builder.Services.AddScoped<IFlightService, FlightService>();

// Реєструє доступ до сховища рейсів на один HTTP-запит.
builder.Services.AddScoped<IFlightRepository, FlightRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Підключає маршрути контролерів.
app.MapControllers();

app.Run();
