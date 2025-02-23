using EmployeeAdminPortal.Data;
using EmployeeAdminPortal.Models.EcommerceModel;
using EmployeeAdminPortal.Models.EcommerceModel.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAdminPortal.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Endpoint: POST /api/orders/CreateOrder
        [HttpPost("CreateOrder")]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] OrderDTO orderDto)
        {
            var customer = await _dbContext.Customers.FindAsync(orderDto.CustomerId);
            if (customer is null)
                return BadRequest("Customer does not exist");

            var order = new Order()
            {
                CustomerId = orderDto.CustomerId,
                OrderDate = DateTime.UtcNow,
                OrderStatus = "Processing",
                OrderAmount = 0,
                OrderItems = new List<OrderItem>(),
            };

            decimal totalAmount = 0;

            foreach (var itm in orderDto.Items)
            {
                var product = await _dbContext.Products.FindAsync(itm.ProductId);

                if (product is null)
                    return BadRequest($"Product with Id {itm.ProductId} deos not exist");

                if (product.Stock < itm.Quantity)
                    return BadRequest($"Insufficient stock for procut {product.Name}");

                product.Stock -= itm.Quantity;
                totalAmount += (itm.Quantity * product.Price);

                var orderItem = new OrderItem()
                {
                    ProductId = itm.ProductId,
                    Quantity = itm.Quantity,
                    UnitPrice = product.Price
                };

                order.OrderItems.Add(orderItem);
            }

            order.OrderAmount = totalAmount;

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
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
    }
}
