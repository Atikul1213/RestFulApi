using Microsoft.EntityFrameworkCore;

namespace EmployeeAdminPortal.Models.EcommerceModel
{
    public class Payment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        [Precision(18, 2)]
        public decimal Amount { get; set; }
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
        public string PaymentMethod { get; set; } = null!;
        public string? TransactionId { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    }
}
