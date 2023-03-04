using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;

namespace WebApplication1.Controllers
{
    [ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		public ICollection<WeatherDetails> WeatherDetailsArray { get; set; }

		private readonly ILogger<WeatherForecastController> _logger;

		public WeatherForecastController(ILogger<WeatherForecastController> logger)
		{
			_logger = logger;

			WeatherDetailsArray = new List<WeatherDetails>();

			WeatherDetailsArray.Add(new WeatherDetails
			{
				Id = 1,
				Country = "India",
				WatherType = "Hot"
			});

			WeatherDetailsArray.Add(new WeatherDetails
			{
				Id = 2,
				Country = "India",
				WatherType = "Humid"
			});

			WeatherDetailsArray.Add(new WeatherDetails
			{
				Id = 3,
				Country = "USA",
				WatherType = "Hot"
			});

			WeatherDetailsArray.Add(new WeatherDetails
			{
				Id = 4,
				Country = "USA",
				WatherType = "Cold"
			});

			WeatherDetailsArray.Add(new WeatherDetails
			{
				Id = 5,
				Country = "Sudan",
				WatherType = "Very Hot"
			});

			WeatherDetailsArray.Add(new WeatherDetails
			{
				Id = 6,
				Country = "Canada",
				WatherType = "Very Cold"
			});

			WeatherDetailsArray.Add(new WeatherDetails
			{
				Id = 7,
				Country = "Canada",
				WatherType = "Mild"
			});
		}

		[HttpGet]
		public IEnumerable<WeatherForecast> Get()
		{
			return Enumerable.Range(1, 5).Select(index => new WeatherForecast
			{
				Date = DateTime.Now.AddDays(index),
				TemperatureC = Random.Shared.Next(-20, 55),
				Summary = Summaries[Random.Shared.Next(Summaries.Length)]
			})
			.ToArray();
		}

		[HttpGet("getAll")]
		public IActionResult GetAllData()
		{
			return Ok(WeatherDetailsArray);
		}

		// Path Parameter example
		[HttpGet("getDataById/{recordId}")]
		public IActionResult GetSpecificData(int recordId)
		{
			return Ok(WeatherDetailsArray.Where(x => x.Id.Equals(recordId)).SingleOrDefault());
		}

		// Path Parameter + Query Parameter example
		[HttpGet("getDataByCountry/{country}")]
		public IActionResult GetSpecificData(string country, [FromQuery] string? searchString)
		{
			var result = WeatherDetailsArray
				.Where(x => x.Country.Equals(country))
				.ToList();

			if (!string.IsNullOrEmpty(searchString))
				result = result.Where(x => x.WatherType.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)).ToList();

			return Ok(result);
		}

		// HTTP POST JSOn Body Example
		[HttpPost()]
		public IActionResult CreateNewRecord(WeatherDataInputDTO model)
		{
			WeatherDetails weatherDetails = new WeatherDetails
			{
				Id = WeatherDetailsArray.Count() + 1,
				Country = model.Country,
				WatherType = model.Weather
			};

			WeatherDetailsArray.Add(weatherDetails);

			return Ok(WeatherDetailsArray);
		}

		// JSON body + path parameter
		[HttpPut("{recordId}")]
		public IActionResult UpdateCurrentRecord(int recordId, WeatherDataInputDTO model)
		{
			var recordFromDb = WeatherDetailsArray.SingleOrDefault(x => x.Id.Equals(recordId));
			if (recordFromDb != null)
			{
				recordFromDb.Country = model.Country;
				recordFromDb.WatherType = model.Weather;
			}

			return Ok(WeatherDetailsArray);
		}

		[HttpDelete("{recordId}")]
		public IActionResult DeleteRecord(int recordId)
		{
			if (recordId == 0)
				return BadRequest("invalid data");

			var recordFromDb = WeatherDetailsArray.SingleOrDefault(x => x.Id.Equals(recordId));
			if (recordFromDb == null)
			{
				return NotFound("record not found");
			}

			WeatherDetailsArray.Remove(recordFromDb);

			return Ok(WeatherDetailsArray);
		}
	}
}