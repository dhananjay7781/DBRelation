using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class UserRole
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        public int RoleId { get; set; }

        [Required]
        public Role Role { get; set; }


        public DateTime CreatedAt { get; set; }

        public UserRole()
        {
            CreatedAt = DateTime.Now;
        }

       // ICollection<Role> Roles { get; set;}
    }
}
