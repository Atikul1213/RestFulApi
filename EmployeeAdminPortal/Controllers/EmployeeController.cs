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
        public IActionResult GetUser([FromRoute] int id)
        {
            return Ok(id);
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] User user)
        {
            return Ok(user);
        }

        [HttpPost]
        public IActionResult UploadFile([FromForm] IFormFile file)
        {
            return Ok(file.FileName);
        }


        [Route("{employeeName}")]
        [HttpGet]
        public string GetEmployeeDetails(string employeeName)
        {
            return $"Response from GetEmployee Details by employeeName: {employeeName} Method";
        }


        [Route("{employeeId: int}")]
        [Route("{employeeId:int:min(100)}")]
        [Route("{employeeId:int:max(100)}")]
        [Route("{employeeId:int:range(1,100)}")]
        [Route("{employeeName:length(5)}")]
        [Route("{employeeName:alpha:minlength(5)}")]
        [Route("{employeeName:alpha:maxlength(5)}")]
        [Route("{employeeName:alpha:minlength(5):maxlength(10)}")]
        [Route("{employeeName:regex(a(b|c))}")]
        [Route("{employeeName:alpha}")]
        [Route("{minPrice:int:min(100)}/to/{maxPrice:int:max(100000)}")]
        [Route("{category:alpha}/{rating:int:range(1,5)}")]
        [HttpGet]
        public string GetEmployeeDetails(int employeeId)
        {
            return $"Response from GetEmployee Details by employeeId: {employeeId} Method";
        }

       
        [Route("employee/all")]
        [HttpGet]
        public string GetAllEmployees()
        {
            return "Response from GetAllEmployees Method";
        }


        [Route("employee/{id}")]
        [HttpGet]
        public string GetEmployeeById(int id)
        {
            return $"Response from GetEmployeeById Method with ID: {id}";
        }


        [Route("employee/department/{department}")]
        [HttpGet]
        public string GetDepartmentEmployee(string department)
        {
            return $"Response from GetDepartmentEmployee Method with Department: {department}";
        }


        [HttpGet("{id}")]
        public IActionResult GetEmployeeById1(int id)
        {
            return Ok(1);
        }


        [HttpGet("Gender/{gender}/City/{city}")]
        public IActionResult GetEmployeeByGenderAndCity(string gender, string city)
        {
            return Ok(1);
        }


        [HttpGet("Search")]
        public IActionResult SearcEmployee([FromQuery] string department)
        {
            // GET api/Employee/Search?Department=HR
            return BadRequest();
        }


        [HttpGet("Search")]
        public IActionResult SearcEmployee([FromQuery] EmployeeSearch searchModel)
        {
            // GET api/Employee/Search?Gender=Male&Department=IT&City=Los Angeles
            return BadRequest();
        }



        // GET api/Employee/Search?Gender=Male&Department=IT&City=Los Angeles
         [HttpGet("Search")]
        public IActionResult EmployeeSearch([FromQuery] string? gender, [FromQuery] string? department, [FromQuery] string? city)
        {
            return NotFound();
        }


        // GET api/Employee/DirectSearch?Gender=Male&Department=IT
        [HttpGet("DirectSearch")]
        public IActionResult DirectSearchEmployees()
        {
            var gender = HttpContext.Request.Query["Gender"].ToString();
            var department = HttpContext.Request.Query["Department"].ToString();
            var city = HttpContext.Request.Query["City"].ToString();

            return BadRequest();
        }


        // GET api/Employee/Gender/Male?Department=IT&City=Los Angeles

        [HttpGet("Gender/{gender}")]
        public IActionResult GetEmployeeByGender1([FromRoute] string gender, [FromQuery] string? department, [FromQuery] string? city)
        {
            return Ok();
        }


        // Action Method with Multiple Routes
        [HttpGet("All")]
        [HttpGet("AllEmployees")]
        [HttpGet("GetAll")]
        public IActionResult GetAllEmployee()
        {
            return Ok();
        }

        [Route("Employee/GetAllEmployees2")]
        [HttpGet]
        public string GetAllEmployees2()
        {
            return "Response from GetAllEmployees Method";
        }
         

         
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


        ///  Overriding a Controller-Level Route Prefix 
        [Route("~/Employee/All")]
        [HttpGet]
        public string GetAllEmployees()
        {
            return "Response from GetAllEmployees Method";
        }

        */
    }
}
