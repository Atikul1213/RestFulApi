using EmployeeAdminPortal.Data;
using EmployeeAdminPortal.Models.EcommerceModel;
using EmployeeAdminPortal.Models.EcommerceModel.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAdminPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public CustomerController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Customer>> RegisterCustomer([FromForm] CustomerRegistrationDTO registrationDto)
        {
            if (await _dbContext.Customers.AnyAsync(c => c.Email == registrationDto.Email))
                return BadRequest("Email already exist");

            var customer = new Customer()
            {
                Name = registrationDto.Name,
                Email = registrationDto.Email,
                Password = registrationDto.Password
            };

            _dbContext.Customers.Add(customer);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromHeader(Name = "X-Client-ID")] string clientId,
                                                [FromBody] CustomerLoginDTO loginDto)
        {
            if (string.IsNullOrEmpty(clientId))
                return BadRequest("Missing X-client-id header");

            var customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.Email == loginDto.Email
                                    && c.Password == loginDto.Password);
            if (customer is null)
            {
                return Unauthorized("Invalid email or password.");
            }
            return Ok(new { Message = "Authentication successfull" });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _dbContext.Customers.FindAsync(id);

            if (customer is null)
                return NotFound();

            return Ok(customer);
        }

    }
}
