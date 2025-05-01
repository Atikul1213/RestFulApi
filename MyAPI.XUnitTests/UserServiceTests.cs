using EmployeeAdminPortal.Repositories;
using EmployeeAdminPortal.Services;
using Moq;

namespace MyAPI.XUnitTests
{
    public class UserServiceTests
    {
        private readonly UserService _deomoService;
        private readonly Mock<IUserRepository> _mockRepository;

        public UserServiceTests()
        {
            _mockRepository = new Mock<IUserRepository>();
            _deomoService = new UserService(_mockRepository.Object);
        }
    }
}
