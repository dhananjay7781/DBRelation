using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class WeatherDataInputDTO
    {
        [Required]
        public string Country { get; set; }

        [Required]
        public string Weather { get; set; }
    }
}
