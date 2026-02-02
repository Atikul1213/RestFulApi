namespace EmployeeAdminPortal.Models.EcommerceModel
{
    public class ProductCategory
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public DateTime LinkedAt { get; set; } = DateTime.UtcNow;
        public Product Product { get; set; } = null!;
        public Category Category { get; set; } = null!;
    }
}
