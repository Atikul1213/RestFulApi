using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EmployeeAdminPortal.Models.EcommerceModel
{
    public class Product
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Product name is required")]
        public string Name { get; set; } = null!;
        public string SKU { get; set; } = null!;
        public string? Description { get; set; }
        [Range(0.01, 10000, ErrorMessage = "Price must be between 0.01 and 10000")]
        [Precision(18, 2)]
        public decimal Price { get; set; }
        [Precision(18, 2)]
        public decimal Discount { get; set; }
        [Precision(18, 2)]
        public decimal FinalPrice { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be a negative value")]
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
