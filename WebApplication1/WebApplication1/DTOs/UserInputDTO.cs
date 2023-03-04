using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
	public class UserInputDTO
	{
		[Required]
		[StringLength(50, ErrorMessage = "First name length cannot exceed 50 characters")]
		public string FirstName { get; set; }

		[Required]
		[StringLength(50, ErrorMessage = "Last name length cannot exceed 50 characters")]
		public string LastName { get; set; }

		[Required]
		[StringLength(50, ErrorMessage = "Username length cannot exceed 50 characters")]
		public string UserName { get; set; }

		[Required]
		[StringLength(200, ErrorMessage = "Email length cannot exceed 200 characters")]
		public string Email { get; set; }

		[Required]
		[StringLength(1000, ErrorMessage = "Password length cannot exceed 1000 characters")]
		public string Password { get; set; }
	}

	public class UserInputDTOV2 : UserInputDTO
	{
		public string ProfilePhotoUrl { get; set; }
	}
    public class UserInputDTOV3 : UserInputDTO
    {
		public ICollection<AddressInputDTO> Addresses { get; set; }
	}

	public class UserInputDTOV4 : UserInputDTO
	{
		public ICollection<RoleInputDTO> Roles { get; set; }
	}


    //Many to Many Relation
    public class UserInputDTOV5

    {
		public int Id { get; set; }
		public ICollection<MultipleInputRoleDTO> MultipleRoles { get; set; }
    }
}
