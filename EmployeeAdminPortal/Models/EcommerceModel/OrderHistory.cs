namespace EmployeeAdminPortal.Models.EcommerceModel
{
    public class OrderHistory
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public OrderStatus OldStatus { get; set; }
        public OrderStatus NewStatus { get; set; }
        public string? Remarks { get; set; }
        public DateTime ChangeAt { get; set; } = DateTime.UtcNow;
    }
}
