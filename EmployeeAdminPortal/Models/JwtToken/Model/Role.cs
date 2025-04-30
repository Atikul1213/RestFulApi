using System.ComponentModel.DataAnnotations;

namespace EmployeeAdminPortal.Models.JwtToken.Model
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
