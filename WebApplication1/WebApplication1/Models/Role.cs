using Azure;

namespace WebApplication1.Models
{
    public class Role
    {
        public int Id { get; set; }

        public string RoleName { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }

       // public ICollection<User> Users { get; set; }
    }
}
