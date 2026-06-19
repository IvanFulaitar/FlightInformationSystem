using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using FlightClientApp.Models;

namespace FlightClientApp.Services;

public class FlightApiClient : IFlightApiClient
{
    private readonly HttpClient _httpClient;

    public FlightApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<FlightViewModel>> GetAllAsync()
    {
        return await GetListAsync("api/flights/all");
    }

    public async Task<FlightViewModel?> GetByNumberAsync(string flightNumber)
    {
        var response = await _httpClient.GetAsync($"api/flights/{Uri.EscapeDataString(flightNumber)}");

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        await EnsureSuccessAsync(response);

        return await response.Content.ReadFromJsonAsync<FlightViewModel>();
    }

    public async Task<List<FlightViewModel>> GetByDateAsync(DateTime date)
    {
        return await GetListAsync($"api/flights?date={date:yyyy-MM-dd}");
    }

    public async Task<List<FlightViewModel>> GetByDepartureCityAndDateAsync(string city, DateTime date)
    {
        var url = $"api/flights/departure?city={Uri.EscapeDataString(city)}&date={date:yyyy-MM-dd}";

        return await GetListAsync(url);
    }

    public async Task<List<FlightViewModel>> GetByArrivalCityAndDateAsync(string city, DateTime date)
    {
        var url = $"api/flights/arrival?city={Uri.EscapeDataString(city)}&date={date:yyyy-MM-dd}";

        return await GetListAsync(url);
    }

    public async Task<List<FlightViewModel>> GetByCityAsync(string city)
    {
        var url = $"api/flights/city?city={Uri.EscapeDataString(city)}";

        return await GetListAsync(url);
    }

    public async Task CreateAsync(FlightViewModel flight)
    {
        var response = await _httpClient.PostAsJsonAsync("api/flights", flight);

        await EnsureSuccessAsync(response);
    }

    public async Task UpdateAsync(string flightNumber, FlightViewModel flight)
    {
        var response = await _httpClient.PutAsJsonAsync(
            $"api/flights/{Uri.EscapeDataString(flightNumber)}",
            flight);

        await EnsureSuccessAsync(response);
    }

    public async Task DeleteAsync(string flightNumber)
    {
        var response = await _httpClient.DeleteAsync($"api/flights/{Uri.EscapeDataString(flightNumber)}");

        await EnsureSuccessAsync(response);
    }

    private async Task<List<FlightViewModel>> GetListAsync(string url)
    {
        var response = await _httpClient.GetAsync(url);

        await EnsureSuccessAsync(response);

        return await response.Content.ReadFromJsonAsync<List<FlightViewModel>>() ?? new List<FlightViewModel>();
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        var message = problem?.Detail ?? problem?.Title;

        if (string.IsNullOrWhiteSpace(message))
        {
            message = $"Request failed with status code {(int)response.StatusCode}.";
        }

        throw new InvalidOperationException(message);
    }
}

