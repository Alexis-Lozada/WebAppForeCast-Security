using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebAppForeCast.Services
{
    public class WeatherService
    {
        private readonly List<WeatherForecast> _forecasts = new();
        private int _nextId = 1;

        public IEnumerable<WeatherForecast> GetAll()
        { 
            return _forecasts;        
        }

        public WeatherForecast? getById(int id)
        { 
            return _forecasts.FirstOrDefault(f => f.Id == id);
        }

        public WeatherForecast Create(WeatherForecast forecast)
        {
            if (forecast == null)
            { 
                throw new ArgumentNullException(nameof(forecast));
            }
            forecast.Id = _nextId++;
            _forecasts.Add(forecast);
            return forecast;
        }

        public bool Update(int id, WeatherForecast updateForecast)
        {
            if (updateForecast == null)
            {
                throw new ArgumentNullException(nameof(updateForecast));
            }
            var existingForecast = getById(id);
            if (existingForecast == null)
            {
                return false;
            }
            existingForecast.Date = updateForecast.Date;
            existingForecast.TemperatureC = updateForecast.TemperatureC;
            existingForecast.Summary = updateForecast.Summary;
            return true;
        }

        public bool Delete(int id)
        { 
            var forecast = getById(id);
            if (forecast == null)
            {
                return false;
            }
            _forecasts.Remove(forecast);
            return true;
        }

    }
}
