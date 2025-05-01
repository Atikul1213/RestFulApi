using EmployeeAdminPortal.Models;
using EmployeeAdminPortal.Repositories;

namespace MyAPI.XUnitTests
{
    public class UserRepositoryTests
    {
        private readonly UserRepository _userRepository;

        public UserRepositoryTests()
        {
            _userRepository = new UserRepository();
        }

        // Test to verify that GetUserById returns the correct user
        [Fact]
        public void GetUserById_ReturnsCorrectUser()
        {
            var userId = 1;
            var result = _userRepository.GetUserById(userId);

            // check result is not null
            Assert.NotNull(result);
            // check that the ID of the returned is correct
            Assert.Equal(userId, result.Id);
        }

        // Test to verify that GetUserById returns null when the user is not found
        [Fact]
        public void GetUserById_ReturnNullWhenUserNotFound()
        {
            // Assume this Id does not exist
            var userId = 99;
            var result = _userRepository.GetUserById(userId);
            Assert.Null(result);
        }

        // Test to verify that GetAllUsers returns all users
        [Fact]
        public void GetAllUsers_ReturnsAllUsers()
        {
            var result = _userRepository.GetAllUsers();

            // Check that the result is not null
            Assert.NotNull(result);
            // Assuming there are 2 users, check that the count is correct
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
            // Arrange
            var userId = 1;
            // Act
            _userRepository.DeleteUser(userId);
            var result = _userRepository.GetUserById(userId);
            // Assert
            Assert.Null(result); // Check that the user was deleted and cannot be found
        }
    }
}
