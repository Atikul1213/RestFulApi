using EmployeeAdminPortal.Models.EcommerceModel;

namespace EmployeeAdminPortal.Services
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts();
        Product? GetProductById(int id);
        IEnumerable<Product> SearchProducts(string? category,
            decimal? minPrice, decimal? maxPrice);
        void InsertProduct(Product product);
        bool UpdateProductPrice(int id, decimal discountPercentage);
    }
}
