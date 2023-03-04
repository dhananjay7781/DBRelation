
namespace WebApplication1.Models
{
	public class Address
	{
		public int Id { get; set; }

		public string AddressLine { get; set; }

		public string City { get; set; }

		public string State { get; set; }

		public string Country { get; set; }

		public Int64 ZipCode { get; set; }

		// Relationships
		// One-to-Many
		// User-Address
		public int UserId { get; set; }

		public User User { get; set; }
	}
}
