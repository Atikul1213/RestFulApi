using EmployeeAdminPortal.Models.EcommerceModel;

namespace EmployeeAdminPortal.Repositories
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(int id);
        Task AddAsync(Order order);
        Task SaveChangesAsync();
    }
}
