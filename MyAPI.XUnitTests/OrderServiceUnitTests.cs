using EmployeeAdminPortal.Models.EcommerceModel;
using EmployeeAdminPortal.Models.EcommerceModel.DTO;
using EmployeeAdminPortal.Repositories;
using EmployeeAdminPortal.Services.TestServices;
using Moq;

namespace MyAPI.XUnitTests
{
    public class OrderServiceUnitTests
    {
        #region Fields

        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<ICustomerRepository> _mockCustomerRepository;
        private readonly IOrderService _orderService;

        #endregion

        #region Ctor
        public OrderServiceUnitTests()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockProductRepository = new Mock<IProductRepository>();
            _mockCustomerRepository = new Mock<ICustomerRepository>();

            _orderService = new OrderService(
                _mockOrderRepository.Object,
                _mockProductRepository.Object,
                _mockCustomerRepository.Object
                );
        }
        #endregion

        #region Methods
        [Fact]
        public async Task CreateOrderAsync_WithValidInput_ReturnOrderResponse()
        {
            int customerId = 1;
            var orderDto = new OrderCreateDTO
            {
                CustomerId = customerId,
                Items = new List<OrderItemCreateDTO>
                {
                    new() { ProductId = 10, Quantity = 2 },
                    new() { ProductId = 20, Quantity = 1 }
                }
            };

            _mockCustomerRepository.Setup(c => c.GetByIdAsync(customerId))
                .ReturnsAsync(new Customer { Id = customerId, FirstName = "Test Customer" });

            _mockProductRepository.Setup(p => p.GetByIdAsync(10))
                .ReturnsAsync(new Product { Id = 10, Name = "Product 10", Price = 50, StockQuantity = 5 });

            _mockProductRepository.Setup(p => p.GetByIdAsync(20))
                .ReturnsAsync(new Product { Id = 20, Name = "Product 20", Price = 100, StockQuantity = 3 });

            _mockOrderRepository.Setup(o => o.AddAsync(It.IsAny<Order>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var result = await _orderService.CreateOrderAsync(orderDto);
            // Assert: The returned DTO should not be null (order was created successfully)
            Assert.NotNull(result);

            // Assert: The CustomerId in the result should equal the one we passed (1)
            Assert.Equal(customerId, result.CustomerId);

            // Assert: The service should return 2 order items (we passed two items)
            Assert.Equal(2, result.OrderItems.Count);

            // Verify that AddAsync(Order) was called exactly once on the mock order repository
            _mockOrderRepository.Verify(o =>
            o.AddAsync(It.IsAny<Order>()), Times.Once);
        }


        [Fact]
        public async Task CreateOrderAsync_CustomerNotFound_ThrowNotFoundException()
        {
            _mockCustomerRepository.Setup(c => c.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Customer?)null);

            var orderDto = new OrderCreateDTO
            {
                CustomerId = 999,
                Items = new List<OrderItemCreateDTO> { new() { ProductId = 1, Quantity = 1 } }
            };


            // Act & Assert: calling CreateOrderAsync should immediately throw NotFoundException

            await Assert.ThrowsAsync<NotFoundException>(() =>
            _orderService.CreateOrderAsync(orderDto));
        }

        // Unit Test for Product Not Found
        [Fact]
        public async Task CreateOrderAsync_ProductNotFound_ThrowNotFoundException()
        {
            int customerId = 1;
            _mockCustomerRepository.Setup(c => c.GetByIdAsync(customerId))
                .ReturnsAsync(new Customer { Id = customerId });

            _mockProductRepository.Setup(p => p.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Product)null);

            var orderDto = new OrderCreateDTO
            {
                CustomerId = customerId,
                Items = new List<OrderItemCreateDTO> { new() { ProductId = 999, Quantity = 1 } }
            };

            await Assert.ThrowsAsync<NotFoundException>(() =>
            _orderService.CreateOrderAsync(orderDto));
        }

        // Unit Test for Insufficient Stock
        [Fact]
        public async Task CreateOrderAsync_InsufficientStock_ThrowsInvalidOperationException()
        {
            int customerId = 1;
            _mockCustomerRepository.Setup(c => c.GetByIdAsync(customerId))
                .ReturnsAsync(new Customer { Id = customerId });

            _mockProductRepository.Setup(p => p.GetByIdAsync(10))
                .ReturnsAsync(new Product { Id = 10, StockQuantity = 1 });

            var orderDto = new OrderCreateDTO
            {
                CustomerId = customerId,
                Items = new List<OrderItemCreateDTO> { new() { ProductId = 10, Quantity = 5 } }
            };


            // Act & Assert: the service should throw InvalidOperationException because 5 > stock(1)
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _orderService.CreateOrderAsync(orderDto));
        }


        // Unit Test for Fetching an Existing Order 
        [Fact]
        public async Task GetOrderByIdAsync_ExistingOrder_ReturnOrderResponse()
        {
            int orderId = 100;
            int customerId = 1;
            var order = new Order
            {
                Id = orderId,
                CustomerId = 1,
                TotalAmouont = 150,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { ProductId = 10, Quantity = 3, UnitPrice = 50, TotalPrice = 150 }
                }
            };

            _mockCustomerRepository.Setup(c => c.GetByIdAsync(customerId))
               .ReturnsAsync(new Customer { Id = customerId });

            _mockOrderRepository.Setup(o => o.GetByIdAsync(orderId))
                .ReturnsAsync(order);

            var result = await _orderService.GetOrderByIdAsync(orderId);
            // Assert: the returned OrderResponseDTO should not be null
            Assert.NotNull(result);
            // Assert: the returned DTO’s OrderId should match the one we passed in
            Assert.Equal(orderId, result.OrderId);
            // Assert: the returned DTO’s CustomerId should be 1
            Assert.Equal(1, result.CustomerId);
            // Assert: Because the Order had exactly one OrderItem, the DTO should have one item
            Assert.Single(result.OrderItems);
        }


        //Unit Test for Fetching a Non-Existent Order 
        [Fact]
        public async Task GetOrderByIdAsync_NonExistingOrder_ReturnNull()
        {
            int missingOrderId = 999;
            _mockOrderRepository.Setup(o => o.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Order?)null);

            var result = await _orderService.GetOrderByIdAsync(missingOrderId);

            Assert.Null(result);
            _mockOrderRepository.Verify(o =>
            o.GetByIdAsync(missingOrderId), Times.Once);
        }

        #endregion

    }
}
