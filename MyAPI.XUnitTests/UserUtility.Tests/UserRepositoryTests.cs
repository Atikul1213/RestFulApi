using EmployeeAdminPortal.Models;
using EmployeeAdminPortal.Repositories;

namespace MyAPI.XUnitTests.Tests
{
    public class UserRepositoryTests
    {
        #region Fields

        private readonly UserRepository _userRepository;

        #endregion

        #region Ctor
        public UserRepositoryTests()
        {
            _userRepository = new UserRepository();
        }
        #endregion

        #region Tests

        // NamingConvention: MethodName_Condition_ExpectedResult
        [Fact]
        public void GetUserById_ReturnsCorrectUser()
        {
            // Arrange - Variable, classes mocks
            var userId = 1;

            //Act - Execute this function
            var result = _userRepository.GetUserById(userId);

            // Assert - Whatever is returned, is it what you expected?
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
        }


        [Fact]
        public void GetUserById_ReturnNullWhenUserNotFound()
        {
            var userId = 99;
            var result = _userRepository.GetUserById(userId);
            Assert.Null(result);
        }


        // Test to verify that GetAllUsers returns all users
        [Fact]
        public void GetAllUsers_ReturnsAllUsers()
        {
            var result = _userRepository.GetAllUsers();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        // Test to verify that AddUser adds a user correctly
        [Fact]
        public void AddUser_AddsUserCorrectly()
        {
            var newUser = new User { Id = 3, Name = "Sam Wilson", Email = "sam@example.com" };
            _userRepository.AddUser(newUser);
            var result = _userRepository.GetUserById(newUser.Id);

            Assert.NotNull(result);
            Assert.Equal(newUser.Id, result.Id);
            Assert.Equal(newUser.Name, result.Name);
            Assert.Equal(newUser.Email, result.Email);
        }


        // Test to verify that UpdateUser updates a user correctly
        [Fact]
        public void UpdateUser_UpdateUserCorrectly()
        {
            var updatedUser = new User { Id = 1, Name = "John Updated", Email = "john.updated@example.com" };

            _userRepository.UpdateUser(updatedUser);
            var result = _userRepository.GetUserById(updatedUser.Id);

            Assert.NotNull(result);
            Assert.Equal(updatedUser.Name, result.Name);
            Assert.Equal(updatedUser.Email, result.Email);
        }

        // Test to verify that DeleteUser deletes a user correctly
        [Fact]
        public void DeleteUser_DeletesUserCorrectly()
        {
            // Arrange - Go get your variable, classes
            var userId = 1;

            // Act - Execute this function
            _userRepository.DeleteUser(userId);
            var result = _userRepository.GetUserById(userId);

            // Assert - Whatever is returned, is it what you expected?
            Assert.Null(result);
        }

        #endregion
    }
}
