using EmployeeAdminPortal.Data;
using EmployeeAdminPortal.Models.EcommerceModel;
using EmployeeAdminPortal.Models.EcommerceModel.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace EmployeeAdminPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        #region Fields

        private readonly ApplicationDbContext _dbContext;

        #endregion

        #region Ctor
        public OrderController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion


        #region Method

        // Endpoint: POST /api/orders/CreateOrder
        [HttpPost("PlaceOrder")]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] OrderCreateDTO orderDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage);

                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Validation failed.",
                        Details = string.Join(" | ", errors)
                    });
                }

                var customer = await _dbContext.Customers.Include(c => c.Addresses)
                    .FirstOrDefaultAsync(c => c.Id == orderDto.CustomerId && c.IsActive == true);

                if (customer == null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Customer not found or inactive"
                    });
                }

                var shippingAddress = customer.Addresses.FirstOrDefault(a =>
                a.Id == orderDto.ShippingAddressId && a.IsActive == true);

                var billingAddress = customer.Addresses.FirstOrDefault(a =>
                a.Id == orderDto.BillingAddressId && a.IsActive == true);

                if (shippingAddress == null || billingAddress == null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Invalid shipping or billing address"
                    });
                }

                var productIds = orderDto.Items.Select(i => i.ProductId).Distinct().ToList();

                var products = await _dbContext.Products.Where(p =>
                productIds.Contains(p.Id) && p.IsActive == true).ToListAsync();

                if (products.Count != productIds.Count)
                {
                    return BadRequest(new
                    {
                        Succsss = false,
                        Message = $"Some product do not exist"
                    });
                }

                decimal orderTotal = 0m;
                List<OrderItem> orderItems = new List<OrderItem>();

                foreach (var item in orderDto.Items)
                {
                    var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                    if (product.StockQuantity < item.Quantity)
                    {
                        return BadRequest(new
                        {
                            Success = false,
                            Message = $"Insufficient stock for product: {product.Name}"
                        });
                    }

                    var lineTotal = product.FinalPrice * item.Quantity;
                    orderTotal += lineTotal;

                    orderItems.Add(new OrderItem
                    {
                        ProductId = product.Id,
                        Quantity = item.Quantity,
                        UnitPrice = product.FinalPrice,
                        TotalPrice = lineTotal
                    });

                    product.StockQuantity -= item.Quantity;
                }

                var order = new Order
                {
                    CustomerId = orderDto.CustomerId,
                    OrderNumber = Guid.NewGuid().ToString()[..8].ToUpper(),
                    OrderDate = DateTime.UtcNow,
                    TotalAmouont = orderTotal,
                    OrderStatus = OrderStatus.Pending,
                    ShippingAddress = $"{shippingAddress.Line1} {shippingAddress.City}",
                    BillingAddress = $"{billingAddress.Line1} {billingAddress.City}"
                };

                await _dbContext.Orders.AddAsync(order);

                foreach (var item in orderItems)
                {
                    item.Order = order;
                    await _dbContext.OrderItems.AddAsync(item);
                }

                var payment = new Payment
                {
                    Order = order,
                    Amount = orderTotal,
                    PaymentStatus = PaymentStatus.Pending,
                    PaymentMethod = orderDto.Payment.PaymentMethod,
                    TransactionId = string.IsNullOrEmpty(orderDto.Payment.TransactionId)
                    ? Guid.NewGuid().ToString() : orderDto.Payment.TransactionId,
                    PaymentDate = DateTime.UtcNow
                };

                await _dbContext.Payments.AddAsync(payment);

                await _dbContext.OrderHistories.AddAsync(new OrderHistory
                {
                    Order = order,
                    OldStatus = OrderStatus.Pending,
                    NewStatus = OrderStatus.Pending,
                    Remarks = "Order created successfully"
                });

                await _dbContext.SaveChangesAsync();


                var oldOrderStatus = order.OrderStatus;
                payment.PaymentStatus = PaymentStatus.Paid;
                order.OrderStatus = OrderStatus.PaymentReceived;

                await _dbContext.OrderHistories.AddAsync(new OrderHistory
                {
                    OrderId = order.Id,
                    OldStatus = oldOrderStatus,
                    NewStatus = OrderStatus.PaymentReceived,
                    Remarks = "Payment verified successfully"
                });

                if (orderDto.MakeSecondSaveFail)
                {
                    throw new Exception("Simulated failure after first SaveChanges.");
                }

                await _dbContext.SaveChangesAsync();

                return Ok(new OrderResponseDTO
                {
                    OrderId = order.Id,
                    OrderNumber = order.OrderNumber,
                    Message = "Order placed successfully using Implicit Trasaction."
                });
            }
            catch (Exception ex)
            {
                try
                {
                    var logEntry = new FailureLog
                    {
                        OrderId = null,
                        MethodName = "PLaceOrderAsync",
                        ClassName = "OrdersController",
                        ErrorMessage = ex.Message,
                        StackTrace = ex.StackTrace ?? "",
                        RequestPayload = JsonSerializer.Serialize(orderDto),
                        Severity = "Error",
                        CustomerId = orderDto.CustomerId
                    };
                    _dbContext.FailureLogs.Add(logEntry);
                }
                catch { }

                return StatusCode(500, new
                {
                    Success = false,
                    Message = "An unexpected error occured while placing the order.",
                    Details = ex.Message
                });
            }

        }

        // Endpoint: GET /api/orders/GetOrderById/{id}
        [HttpGet("GetOrderById/{id}")]
        public async Task<ActionResult<Order>> GetOrderById([FromRoute] int id)
        {
            var order = await _dbContext.Orders.Include(o => o.OrderItems)
                               .ThenInclude(oi => oi.Product)
                                .Include(o => o.Customer).FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound();

            return Ok(order);
        }

        #endregion
    }
}
