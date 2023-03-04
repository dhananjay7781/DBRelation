namespace WebApplication1.Models
{
	public class ProfilePhoto
	{
		public int Id { get; set; }

		public string Url { get; set; }

		// Relationships
		// One-to-One
		// User-ProfilePhoto
		public int UserId { get; set; }

		public User User { get; set; }

		//One to Many
		//User Adress
	}
}
