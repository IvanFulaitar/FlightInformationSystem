using FlightStorageService.Services;
using FlightStorageService.Repositories;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Реєструємо сервіс у DI-контейнері.
// Коли ASP.NET побачить IFlightService у конструкторі,
// він автоматично створить FlightService і передасть його.
//
// Scoped = один екземпляр сервісу на один HTTP-запит.
builder.Services.AddScoped<IFlightService, FlightService>();

// Реєструємо репозиторій у DI-контейнері.
// Коли FlightService попросить IFlightRepository,
// ASP.NET створить FlightRepository і передасть його в сервіс.
builder.Services.AddScoped<IFlightRepository, FlightRepository>();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// У мене тут будуть контролери,
// які будуть обробляти запити від клієнтів
// і взаємодіяти з базою даних для зберігання інформації про рейси.
app.MapControllers();

app.Run();