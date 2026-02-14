using EmployeeAdminPortal.Models.EcommerceModel;

namespace EmployeeAdminPortal.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByIdAsync(int id);
    }
}
