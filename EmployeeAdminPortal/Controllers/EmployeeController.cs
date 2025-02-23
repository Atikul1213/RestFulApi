using EmployeeAdminPortal.Data;
using EmployeeAdminPortal.Entities;
using EmployeeAdminPortal.Models.EmployeeCon;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAdminPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public EmployeeController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            var employees = _db.Employees.ToList();

            return Ok(employees);
        }


        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetEmployeeById(int id)
        {
            var employee = _db.Employees.Find(id);

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }


        [HttpPost]
        public IActionResult AddEmployee(AddEmployeeDto addEmployeeDto)
        {
            var employeeEntity = new Employee()
            {
                Name = addEmployeeDto.Name,
                Email = addEmployeeDto.Email,
                Phone = addEmployeeDto.Phone,
                Salary = addEmployeeDto.Salary
            };

            _db.Employees.Add(employeeEntity);
            _db.SaveChanges();

            return Ok(employeeEntity);
        }



        [HttpPut]
        [Route("{id:int}")]
        public IActionResult UpdateEmployee(int id, UpdateEmployeeDto updateEmployeeDto)
        {
            var employee = _db.Employees.Find(id);

            if (employee == null)
                return NotFound();

            employee.Name = updateEmployeeDto.Name;
            employee.Email = updateEmployeeDto.Email;
            employee.Phone = updateEmployeeDto.Phone;
            employee.Salary = updateEmployeeDto.Salary;

            _db.Employees.Update(employee);
            _db.SaveChanges();

            return Ok(employee);
        }



        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult DeleteEmployee(int id)
        {
            var employee = _db.Employees.Find(id);

            if (employee == null)
                return NotFound();

            _db.Employees.Remove(employee);
            _db.SaveChanges();

            return Ok();
        }

        /*
         
        [HttpGet]
        public IActionResult GetUser([FromQuery] int userId)
        {
            return Ok(userId);
        }

        [HttpGet("/user/{id}")]
        public IActionResult GetUserDetails([FromRoute] int id)
        {

            return Ok(id);
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(user);
        }

        [HttpPost]
        public IActionResult UploadFile([FromForm] IFormFile file)
        {
            return Ok(file.FileName);
        }

        [HttpGet]
        public IActionResult GetHeader([FromHeader] string customHeader)
        {
            return Ok(customHeader);
        }
        */

    }
}
