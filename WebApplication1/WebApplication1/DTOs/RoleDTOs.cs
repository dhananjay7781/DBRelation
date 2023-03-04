using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class RoleInputDTO
    {
        [Required]
        public string RoleName { get; set; }
    }

    public class UserRoleAssociationDTO
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int RoleId { get; set; }
    }

    public class RoleOutputDTO
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; }
    }
}
