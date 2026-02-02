using System.ComponentModel.DataAnnotations;

namespace EmployeeAdminPortal.Models.EcommerceModel
{
    public class Customer
    {
        public int Id { get; set; }
        public string CustomerNumeber { get; set; } = null!;
        [Required(ErrorMessage = "Customer name is required. ")]
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string Email { get; set; }
        public string Password { get; set; } = string.Empty;
        public string Phone { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Address> Addresses { get; set; } = new List<Address>();
        public CustomerProfile? Profile { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
