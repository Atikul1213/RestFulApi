using EmployeeAdminPortal.Controllers;
using EmployeeAdminPortal.Models;
using EmployeeAdminPortal.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace MyAPI.XUnitTests
{
    public class UsersControllerTests
    {
        private readonly UsersController _controller;
        private readonly Mock<IUserService> _mockService;
        public UsersControllerTests()
        {
            _mockService = new Mock<IUserService>();
            _controller = new UsersController(_mockService.Object);
        }


        // Test to verify that GetUserById returns an OkObjectResult with the correct user
        [Fact]
        public void GetUser_ReturnsOkResultWithUser()
        {
            var userId = 1;
            var expectedUser = new User { Id = 1, Name = "John Doe", Email = "john@example.com" };

            _mockService.Setup(service => service.GetUserById(userId)).Returns(expectedUser);

            var result = _controller.GetUserById(userId) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(expectedUser, result.Value);
        }


        // Test to verify that GetUserById returns a NotFoundResult when the user is not found
        public void GetUser_ReturnsNotFoundWhenUserNotFound()
        {
            var userId = 90;
            _mockService.Setup(service =>
            service.GetUserById(userId)).Returns((User)null);

            var result = _controller.GetUserById(userId) as NotFoundResult;

            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }


        // Test to verify that GetAllUsers returns an OkObjectResult with a list of users
        [Fact]
        public void GetAllUsers_ReturnsOkResultWithLIstOfUsers()
        {
            var expectedUsers = new List<User>
            {
                new User { Id = 1, Name = "John Doe", Email = "john@example.com" },
                new User { Id = 2, Name = "Jane Smith", Email = "jane@example.com" }
            };

            _mockService.Setup(service => service.GetAllUsers()).Returns(expectedUsers);

            var result = _controller.GetAllUsers() as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(expectedUsers, result.Value);
        }


        // Test to verify that AddUser returns a CreatedAtActionResult
        [Fact]
        public void AddUser_ReturnsCreateAtAction()
        {
            var newUser = new User { Id = 3, Name = "Sam Wilson", Email = "sam@example.com" };

            var result = _controller.AddUser(newUser) as CreatedAtActionResult;

            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal("GetUserById", result.ActionName);
            Assert.Equal(newUser.Id, ((User)result.Value).Id);
        }


        // Test to verify that UpdateUser returns a NoContentResult
        [Fact]
        public void UpdateUser_ReturnsNoContent()
        {
            var updateUser = new User { Id = 1, Name = "John Updated", Email = "john.updated@example.com" };
            _mockService.Setup(service => service.UpdateUser(updateUser));

            var result = _controller.UpdateUser(1, updateUser) as NoContentResult;

            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);
        }

        // Test to verify that DeleteUser returns a NoContentResult
        [Fact]
        public void DeleteUser_ReturnsNoContent()
        {
            var userId = 1;
            _mockService.Setup(service => service.DeleteUser(userId));

            var result = _controller.DeleteUser(userId) as NoContentResult;

            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);
        }
    }
}
