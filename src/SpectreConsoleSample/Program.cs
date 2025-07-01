using SpectreConsoleSample.Utils;
using System.Text.Json;

// Get the OpenWeather API key
string apiKey =
    ConsoleHelper.GetString(
        "Please enter your OpenWeather API key:",
        showHeader: true);

// Get the city name
var city =
    ConsoleHelper.GetString(
        "Please enter the city name:",
        "Pforzheim",
        showHeader: true);

// Make the API request
using HttpClient httpClient = new();
string url = $"https://api.openweathermap.org/data/2.5/weather" +
    $"?q={city}&appid={apiKey}&units=metric";

try
{
    HttpResponseMessage response = await httpClient.GetAsync(url);

    if (response.IsSuccessStatusCode)
    {
        // Read the response content
        var content =
            await response.Content.ReadAsStringAsync();

        // Parse the JSON response
        JsonElement weatherData =
            JsonSerializer.Deserialize<JsonElement>(content);

        // Display the weather data in the console
        ConsoleHelper.ShowHeader();
        ConsoleHelper.WriteJson(weatherData, $"Weather in {city}");

        // Display additional information
        ConsoleHelper.WriteToConsoleLine("\nPress any key to exit...");
        Console.ReadKey(true);
    }
    else
    {
        // If the request failed, read the error content
        string errorContent =
            await response.Content.ReadAsStringAsync();

        // Display an error message if the request failed
        ConsoleHelper.WriteErrorToConsoleLine(
            $"Error fetching weather data: {errorContent}");
    }

}
catch (Exception ex)
{
    // Handle exceptions and display an error message
    ConsoleHelper.WriteErrorToConsoleLine(
        $"Exception occurred: {ex.Message}");
}