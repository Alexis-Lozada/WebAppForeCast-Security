using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAppForeCast.Services;

namespace WebAppForeCast.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly WeatherService _service;

        private readonly Logerservice _logerservice;

		public WeatherForecastController(ILogger<WeatherForecastController> logger, WeatherService service, Logerservice logservice)
        {
            _logger = logger;
            _service = service;
			_logerservice = logservice;
		}

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> Get()
        {
            await _logerservice.LogAsync("INFORMATION", "Get all time previtions.");
			return Ok(_service.GetAll());
        }

        [HttpGet("{id:int}", Name = "GetWeatherForecastById")]
        public async Task<ActionResult<WeatherForecast>> Get(int id)
        {
            var forecast = _service.getById(id);
            if (forecast == null)
            {
				await _logerservice.LogAsync("WARNING", $"Time prevition with ID {id} not found.");
				return NotFound();
            }
			await _logerservice.LogAsync("INFORMATION", $"Get time previtions with ID {id}.");
			return Ok(forecast);
        }

        [HttpPut("{id:int}", Name = "UpdateWeatherForecast")]
        public IActionResult Update(int id, WeatherForecast updateForecast)
        {
            if (!_service.Update(id, updateForecast))
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            if (!_service.Delete(id))
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPost(Name = "PostWeatherForescast")]
        public ActionResult<WeatherForecast> Create(WeatherForecast forecast)
        {
            var createdForecast = _service.Create(forecast);
            return CreatedAtAction(nameof(Get), new { id = createdForecast.Id }, createdForecast);
        }
    }
}
