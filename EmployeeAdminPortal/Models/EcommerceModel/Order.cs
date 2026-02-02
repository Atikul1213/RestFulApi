using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EmployeeAdminPortal.Models.EcommerceModel
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = null!;
        [Required]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;
        [Precision(18, 2)]
        public decimal TotalAmouont { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        [Required]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public string ShippingAddress { get; set; } = null!;
        public string BillingAddress { get; set; } = null!;
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public Payment? Payment { get; set; }
        public ICollection<OrderHistory> History { get; set; } = new List<OrderHistory>();
    }
}
