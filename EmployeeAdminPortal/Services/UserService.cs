using EmployeeAdminPortal.Models;
using EmployeeAdminPortal.Repositories;

namespace EmployeeAdminPortal.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public void AddUser(User user)
        {
            _userRepository.AddUser(user);
        }

        public void DeleteUser(int userId)
        {
            _userRepository.DeleteUser(userId);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public User GetUserById(int userId)
        {
            return _userRepository.GetUserById(userId);
        }

        public void UpdateUser(User user)
        {
            _userRepository.UpdateUser(user);
        }
    }
}
