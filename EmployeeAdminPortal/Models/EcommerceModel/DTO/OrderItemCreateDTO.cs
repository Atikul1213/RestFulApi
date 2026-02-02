using System.ComponentModel.DataAnnotations;

namespace EmployeeAdminPortal.Models.EcommerceModel.DTO
{
    public class OrderItemCreateDTO
    {
        [Required(ErrorMessage = "Product is required")]
        public int ProductId { get; set; }
        [Range(1, 1000, ErrorMessage = "Quantity must be between 1 and 1000")]
        public int Quantity { get; set; }
    }
}
