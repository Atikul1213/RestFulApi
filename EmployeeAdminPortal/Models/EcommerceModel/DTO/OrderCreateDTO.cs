using System.ComponentModel.DataAnnotations;

namespace EmployeeAdminPortal.Models.EcommerceModel.DTO
{
    public class OrderCreateDTO
    {
        [Required(ErrorMessage = "CustomerId is required.")]
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "Shipping Address id is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Shipping address is must be positive value")]
        public int ShippingAddressId { get; set; }
        [Required(ErrorMessage = "Billing Address id is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Billing address is must be positive value")]
        public int BillingAddressId { get; set; }

        [Required(ErrorMessage = "At least one order item is required")]
        [MinLength(1, ErrorMessage = "At least one order item is required")]
        public List<OrderItemCreateDTO> Items { get; set; } = new();
        [Required(ErrorMessage = "Payment details are requied")]
        public PaymentCreateDTO Payment { get; set; } = null!;
        public bool MakeSecondSaveFail { get; set; } = false;
    }
}
