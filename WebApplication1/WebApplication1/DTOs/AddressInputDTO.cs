using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class AddressInputDTO
    {
        public int Id { get; set; }

        [Required]
        public string AddressLine1 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public Int64 ZipCode { get; set; }
    }
}
