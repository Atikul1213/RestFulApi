namespace EmployeeAdminPortal.Models.EcommerceModel.DTO
{
    public class OrderResponseDTO
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = null!;
        public string Message { get; set; } = null!;
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal BaseAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemResponseDTO> OrderItems { get; set; } = new();
    }
}
