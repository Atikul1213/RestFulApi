namespace EmployeeAdminPortal.Models.EcommerceModel.DTO
{
    public class OrderResponseDTO
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}
