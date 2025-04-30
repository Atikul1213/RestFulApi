using System.ComponentModel.DataAnnotations;

namespace EmployeeAdminPortal.Models.JwtToken.Model
{
    public class Client
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string ClientId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(200)]
        public string ClientURL { get; set; }
    }
}
