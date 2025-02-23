using System.ComponentModel.DataAnnotations;

namespace EmployeeAdminPortal.Models
{
    public class User
    {
        [Required]
        public string Name { get; set; }
        [Range(18, 100)]
        public int Age { get; set; }
    }
}
