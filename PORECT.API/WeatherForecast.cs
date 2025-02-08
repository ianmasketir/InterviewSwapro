namespace PORECT.API
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }

    public interface IWeatherService
    {
        List<WeatherForecast> ListWeatherForecast();
    }
    public class WeatherService : IWeatherService
    {
        public List<WeatherForecast> ListWeatherForecast()
        {
            return new List<WeatherForecast>
            {
                new WeatherForecast { Date = DateTime.Now, TemperatureC = 25, Summary = "Sunny" }
            };
        }
    }
}
