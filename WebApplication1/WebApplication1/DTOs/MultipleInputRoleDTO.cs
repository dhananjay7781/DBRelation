using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class MultipleInputRoleDTO
    {
        [Required]
        public int RoleId { get; set; }

        [Required]
        public string RoleName { get; set; }
    }
}
