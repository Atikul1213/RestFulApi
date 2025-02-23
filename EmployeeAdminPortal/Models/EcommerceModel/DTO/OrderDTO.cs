using System.ComponentModel.DataAnnotations;

namespace EmployeeAdminPortal.Models.EcommerceModel.DTO
{
    public class OrderDTO
    {
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public List<OrderItemDTO> Items { get; set; }
    }
}
