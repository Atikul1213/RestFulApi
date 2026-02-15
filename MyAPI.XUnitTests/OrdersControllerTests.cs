using EmployeeAdminPortal.Controllers;
using EmployeeAdminPortal.Data;
using EmployeeAdminPortal.Models.EcommerceModel.DTO;
using EmployeeAdminPortal.Services.TestServices;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace MyAPI.XUnitTests
{
    public class OrdersControllerTests
    {
        #region Fields

        private readonly Mock<IOrderService> _mockOrderService;
        private readonly OrderController _controller;
        private readonly ApplicationDbContext _dbContext;

        #endregion

        #region Ctor
        public OrdersControllerTests()
        {
            _mockOrderService = new Mock<IOrderService>();
            _controller = new OrderController(_dbContext, _mockOrderService.Object);
        }

        #endregion

        #region Methods

        [Fact]
        public async Task GetOrder_ExistingId_ReturnsOkWithOrder()
        {
            var orderId = 1;

            var orderResponse = new OrderResponseDTO
            {
                OrderId = orderId,
                CustomerId = 123
            };

            _mockOrderService.Setup(o => o.GetOrderByIdAsync(orderId))
                .ReturnsAsync(orderResponse);

            var result = await _controller.GetOrder(orderId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(orderResponse, okResult.Value);
        }

        [Fact]
        public async Task GetOrder_NonExstingId_ReturnNotFound()
        {
            _mockOrderService.Setup(o => o.GetOrderByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((OrderResponseDTO?)null);

            var result = await _controller.GetOrder(999);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateOrder_ValidModel_ReturnOkResult()
        {
            var orderDto = new OrderCreateDTO
            {
                CustomerId = 1
            };

            var createOrder = new OrderResponseDTO
            {
                OrderId = 1,
                CustomerId = 1
            };

            _mockOrderService.Setup(o => o.CreateOrderAsync(orderDto))
                .ReturnsAsync(createOrder);

            var result = await _controller.OrderCreate(orderDto);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(createOrder, okResult.Value);
        }

        [Fact]
        public async Task CreateOrder_InvalidModel_ReturnsBadRequest()
        {
            _controller.ModelState.AddModelError("CustomerId", "Required");

            var result = await _controller.OrderCreate(new OrderCreateDTO());
            var bdRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.NotNull(bdRequestResult.Value);
        }
        #endregion
    }
}
