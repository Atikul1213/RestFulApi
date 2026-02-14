using EmployeeAdminPortal.Models.EcommerceModel;
using EmployeeAdminPortal.Models.EcommerceModel.DTO;
using EmployeeAdminPortal.Repositories;

namespace EmployeeAdminPortal.Services.TestServices
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;
        public OrderService(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            ICustomerRepository customerRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
        }

        public async Task<OrderResponseDTO?> CreateOrderAsync(OrderCreateDTO orderCreateDto)
        {
            var customer = await _customerRepository.GetByIdAsync(orderCreateDto.CustomerId);

            if (customer == null)
                throw new NotFoundException($"Customer with {orderCreateDto.CustomerId} not found");

            if (orderCreateDto.Items == null || !orderCreateDto.Items.Any())
                throw new ArgumentException("Order must have at least one item");

            try
            {
                var order = new Order
                {
                    CustomerId = customer.Id,
                    OrderDate = DateTime.UtcNow,
                    OrderItems = new List<OrderItem>()
                };

                decimal baseAmount = 0;

                foreach (var item in orderCreateDto.Items)
                {
                    var product = await _productRepository.GetByIdAsync(item.ProductId);
                    if (product == null)
                        throw new NotFoundException($"Product not found {item.ProductId}");

                    if (item.Quantity <= 0)
                        throw new ArgumentException("Quantity must be greater than 0");

                    if (product.StockQuantity < item.Quantity)
                        throw new InvalidOperationException("Not enough stock");

                    decimal lineTotal = product.FinalPrice * item.Quantity;

                    order.OrderItems.Add(new OrderItem
                    {
                        ProductId = product.Id,
                        Quantity = item.Quantity,
                        UnitPrice = product.FinalPrice,
                        TotalPrice = lineTotal
                    });

                    baseAmount += lineTotal;

                    product.StockQuantity -= item.Quantity;
                }

                order.TotalAmouont = baseAmount;

                await _orderRepository.AddAsync(order);
                await _productRepository.SaveChangesAsync();
                await _orderRepository.SaveChangesAsync();

                return MapToOrderDto(order, customer);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<OrderResponseDTO?> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                return null;

            var customer = await _customerRepository.GetByIdAsync(order.CustomerId);

            return MapToOrderDto(order, customer);
        }


        private OrderResponseDTO MapToOrderDto(Order order, Customer customer)
        {

            return new OrderResponseDTO
            {
                OrderId = order.Id,
                CustomerId = customer.Id,
                CustomerName = customer?.FirstName ?? string.Empty,
                CustomerEmail = customer?.Email ?? string.Empty,
                OrderDate = order.OrderDate,
                BaseAmount = order.TotalAmouont,
                TotalAmount = order.TotalAmouont,
                OrderItems = order.OrderItems.Select(item => new OrderItemResponseDTO
                {
                    OrderItemId = item.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    LineTotal = item.TotalPrice
                }).ToList()
            };
        }
    }
}
