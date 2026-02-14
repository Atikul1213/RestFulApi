using EmployeeAdminPortal.Models.EcommerceModel;

namespace EmployeeAdminPortal.Repositories
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task SaveChangesAsync();
    }
}
