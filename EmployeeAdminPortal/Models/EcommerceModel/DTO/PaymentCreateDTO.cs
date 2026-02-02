using System.ComponentModel.DataAnnotations;

namespace EmployeeAdminPortal.Models.EcommerceModel.DTO
{
    public class PaymentCreateDTO
    {
        [Required(ErrorMessage = "Payment method is required.")]
        [StringLength(50, ErrorMessage = "Payment method is too long")]
        public string PaymentMethod { get; set; } = null!;
        [StringLength(100, ErrorMessage = "Transacttion is too long.")]
        public string? TransactionId { get; set; }
    }
}
