using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
	public class UserAddressUpdateDTO
	{
		[Required]
		[StringLength(500, ErrorMessage = "Address length cannot exceed 500 characters")]
		public string Address { get; set; }
	}
}
