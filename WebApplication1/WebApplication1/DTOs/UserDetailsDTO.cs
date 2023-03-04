using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;

namespace WebApplication1.DTOs
{
    public class UserDetailsDTO
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public DateTime? BirthDate { get; set; }

        public static implicit operator UserDetailsDTO(Role v)
        {
            throw new NotImplementedException();
        }

        public static implicit operator UserDetailsDTO(User v)
        {
            throw new NotImplementedException();
        }
    }

    public class UserDetailsDTOV2 : UserDetailsDTO
    {
        public string ProfilePhotoUrl { get; set; }
    }

    public class UserDetailsDTOV3
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public IEnumerable<string> Roles { get; set; }

        public UserDetailsDTOV3()
        {
            Roles = new List<string>();
        }
    }
}
