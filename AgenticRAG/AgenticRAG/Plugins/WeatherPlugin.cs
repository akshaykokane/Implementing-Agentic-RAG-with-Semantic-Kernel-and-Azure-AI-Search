using Microsoft.SemanticKernel;
using System.ComponentModel;

public class WeatherPlugin
{
    // Simulated API call 
    [KernelFunction, Description("Get current weather for a given city")]
    public static string GetCurrentWeather(
        [Description("Name of the city to get weather for")] string city)
    {
        // Example hardcoded response – replace with actual API call
        return $"The weather in {city} is sunny with 25°C.";
    }
}
