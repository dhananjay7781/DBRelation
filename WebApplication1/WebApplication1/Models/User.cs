using System.ComponentModel.DataAnnotations;
using WebApplication1.DTOs;

namespace WebApplication1.Models
{
	public class User
	{
		public int Id { get; set; }

		public string UserName { get; set; }

		public string Email { get; set; }

		public string FirstName { get; set; }

		public string MiddleName { get; set; }


        public string LastName { get; set; }

		public string Password { get; set; }

		public DateTime? BirthDate { get; set; }

		public string Address { get; set; }

		// Relationships
		// One-to-One
		// User-ProfilePhoto,
		public ProfilePhoto ProfilePhoto { get; set; }

		// One-to-Many
		// User-Address
		public ICollection<Address> Addresses { get; set; }

        //Many-to-Many
        //User Roles
		public ICollection<UserRole> UserRoles { get; set; }


		//One -To-Many
	//	public IEnumerable<AddressInputDTO> Adresses1 { get; set; }




		//Many to Many
		//public ICollection<Role> Roles { get; set; }
	}
}
