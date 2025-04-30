using System.ComponentModel.DataAnnotations;

namespace EmployeeAdminPortal.Models.JwtToken.Model
{
    public class SigningKey
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string KeyId { get; set; }
        [Required]
        public string PrivateKey { get; set; }
        [Required]
        public string PublicKey { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public DateTime CreateAt { get; set; }
        [Required]
        public DateTime ExpireAt { get; set; }
    }
}
