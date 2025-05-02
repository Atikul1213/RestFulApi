using EmployeeAdminPortal.Models;
using EmployeeAdminPortal.Repositories;
using EmployeeAdminPortal.Services;
using Moq;

namespace MyAPI.XUnitTests
{
    public class UserServiceTests
    {
        private readonly UserService _demoService;
        private readonly Mock<IUserRepository> _mockRepository;

        public UserServiceTests()
        {
            _mockRepository = new Mock<IUserRepository>();
            _demoService = new UserService(_mockRepository.Object);
        }

        // Test to verify that GetUserById returns the correct user
        [Fact]
        public void GetUserById_ReturnsUser()
        {
            var userId = 1;
            var expectedUser = new User { Id = 1, Name = "John Doe", Email = "john@example.com" };

            _mockRepository.Setup(repo =>
            repo.GetUserById(userId)).Returns(expectedUser);

            var result = _demoService.GetUserById(userId);

            Assert.NotNull(result);
            Assert.Equal(expectedUser.Id, result.Id);
            Assert.Equal(expectedUser.Name, result.Name);
            Assert.Equal(expectedUser.Email, result.Email);
        }


        // Test to verify that GetUserById returns null when the user is not found
        [Fact]
        public void GetUserById_ReturnsNullWhenUserNotFound()
        {
            var userId = 99;
            _mockRepository.Setup(repo =>
            repo.GetUserById(userId)).Returns((User)null);

            var result = _demoService.GetUserById(userId);
            Assert.Null(result);
        }

        // Test to verify that GetAllUsers returns a list of users
        [Fact]
        public void GetAllUsers_ReturnsListOfUsers()
        {
            var expectedUsers = new List<User>()
            {
                new User { Id = 1, Name = "John Doe", Email = "john@example.com" },
                new User { Id = 2, Name = "Jane Smith", Email = "jane@example.com" }
            };

            _mockRepository.Setup(repo => repo.GetAllUsers()).Returns(expectedUsers);

            var result = _demoService.GetAllUsers();
            Assert.NotNull(result);
            Assert.Equal(expectedUsers.Count, result.Count());
        }


        // Test to verify that AddUser calls the repository's AddUser method
        [Fact]
        public void AddUser_CallsRepository()
        {
            var newUser = new User { Id = 3, Name = "Sam Wilson", Email = "sam@example.com" };
            _demoService.AddUser(newUser);
            _mockRepository.Verify(repo => repo.AddUser(newUser), Times.Once);
        }


        // Test to verify that UpdateUser calls the repository's UpdateUser method
        [Fact]
        public void UpdateUser_CallsRepository()
        {
            var updateUser = new User { Id = 1, Name = "John Updated", Email = "john.updated@example.com" };
            _demoService.UpdateUser(updateUser);
            _mockRepository.Verify(repo => repo.UpdateUser(updateUser), Times.Once);
        }

        // Test to verify that DeleteUser calls the repository's DeleteUser method
        [Fact]
        public void DeleteUser_CallsRepository()
        {
            var userId = 1;
            _demoService.DeleteUser(userId);

            _mockRepository.Verify(repo => repo.DeleteUser(userId), Times.Once);
        }

    }
}
