using EmployeeAdminPortal.Models;

namespace EmployeeAdminPortal.Services
{
    public interface IUserService
    {
        User GetUserById(int userId);
        IEnumerable<User> GetAllUsers();
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int userId);
    }
}
