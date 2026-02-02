using EmployeeAdminPortal.Data;
using EmployeeAdminPortal.Models.EcommerceModel;
using EmployeeAdminPortal.Models.EcommerceModel.DTO;
using EmployeeAdminPortal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace EmployeeAdminPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        #region Fields

        private readonly ApplicationDbContext _dbContext;
        private readonly IProductService _productService;

        #endregion

        #region Ctor

        public ProductController(ApplicationDbContext dbContext,
            IProductService productService)
        {
            _dbContext = dbContext;
            _productService = productService;
        }

        #endregion

        #region Methods

        // Endpoint: GET /api/products/GetProducts?name={name}&category={category}&minPrice={minPrice}&maxPrice={maxPrice}
        [HttpGet("GetProducts")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(
            [FromQuery] string? name,
            [FromQuery] string? category,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery(Name = "Dept")] string department)
        {

            var query = _dbContext.Products.AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(p => p.Name.Contains(name));

            //if (!string.IsNullOrEmpty(category))
            //    query = query.Where(p => p.ProductCategories.Contains(category));

            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice);

            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice);

            var product = await query.ToListAsync();

            return Ok(product);
        }


        [HttpGet("GetProductById/{id}")]
        public async Task<ActionResult<Product>> GetProductById([FromRoute] int id)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if (product is null)
                return NotFound();

            return Ok(product);
        }


        // Endpoint: POST /api/products/CreateProduct
        [HttpPost("CreateProduct")]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] ProductCreateDTO productCreateDto)
        {
            var product = new Product()
            {
                Name = productCreateDto.Name,
                Description = productCreateDto.Description,
                Price = productCreateDto.Price,
                StockQuantity = productCreateDto.Stock
            };

            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        // Endpoint: GET: /api/products/paged?pageNumber={pageNumber}&pageSize={pageSize}
        [HttpGet("paged")]
        public async Task<ActionResult<List<Product>>> GetProductsPaged([FromQuery] int pageNumber = 1,
                                            [FromQuery] int pageSize = 5)
        {
            var products = await _dbContext.Products.Skip((pageNumber - 1) * pageSize)
                                                     .Take(pageSize).AsNoTracking().ToListAsync();

            return Ok(products);
        }

        // Endpoint: POST /api/products/{id}/upload
        [HttpPost("{id}/upload")]
        public async Task<IActionResult> UploadProductImage([FromRoute] int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file Uploaded.");

            var product = await _dbContext.Products.FindAsync(id);
            if (product is null)
                return NotFound();

            var fileName = Path.GetFileName(file.FileName);
            return Ok(new { Message = "Image uploaded successfully.", FileName = fileName });
        }



        // Mix of Multiple Parameters (Route + Query + Header + Service)
        // --------------------------------------------------------------
        // PUT /api/products/discount/2?discountPercent=10
        // Header: X-Api-Key: secret123
        [HttpPut("discount/{id}")]
        public IActionResult ApplyDiscount(
            [FromRoute] int id,
            [FromQuery] decimal discountPercent,
            [FromHeader(Name = "X-Api-Key")] string apiKey,
            [FromServices] IProductService productService)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
                return Unauthorized("Missing API key");

            if (apiKey != "secret123")
                return Unauthorized("Invalid API key.");

            if (discountPercent <= 0)
                return BadRequest("Discount percent must be greater than 0");

            return Ok($"Product # {id} updated successfully.");
        }





        //GET /api/products/details/2
        [HttpGet("details/{id}")]
        public IActionResult GetProductById()
        {
            var idValue = HttpContext.Request.RouteValues["id"]?.ToString();

            if (!int.TryParse(idValue, out int productId))
                return BadRequest("Invalid Product id");

            var product = _productService.GetProductById(productId);

            if (product == null)
                return NotFound($"Product with id {productId} not found.");

            return Ok(product);
        }


        // GET /api/products/search?category=Electronics&minPrice=2000&maxPrice=70000
        [HttpGet("search")]
        public IActionResult SearchProducts()
        {
            string? category = HttpContext.Request.Query["category"];
            decimal.TryParse(HttpContext.Request.Query["minPrice"], out decimal minPrice);
            decimal.TryParse(HttpContext.Request.Query["maxPrice"], out decimal maxPrice);

            var products = _productService.SearchProducts(category,
                minPrice > 0 ? minPrice : null,
                maxPrice > 0 ? maxPrice : null);

            return Ok(products);
        }


        // GET /api/products/all
        // Header: X-Api-Key: secret123
        [HttpGet("all")]

        // public IActionResult GetAllProducts([FromHeader(Name ="X-Api-Key")] string apiKey)
        public IActionResult GetAllProducts()
        {
            var apiKey = HttpContext.Request.Headers["X-Api-Key"].ToString();

            if (string.IsNullOrWhiteSpace(apiKey))
                return Unauthorized("Missing API key");

            if (apiKey != "secret123")
                return Unauthorized("Invalid API key");

            var products = _productService.GetAllProducts();

            return Ok(products);
        }


        // POST /api/products/add
        [HttpPost("addProduct")]
        public async Task<IActionResult> AddProduct()
        {
            string body;
            using (var reader = new StreamReader(HttpContext.Request.Body))
                body = await reader.ReadToEndAsync();

            if (string.IsNullOrWhiteSpace(body))
                return BadRequest("Request body is empty");

            Product? product;
            try
            {
                product = JsonSerializer.Deserialize<Product>(body);
            }
            catch
            {
                return BadRequest("Invalid JSON format");
            }

            if (product == null)
                return BadRequest("Product data is null");

            _productService.InsertProduct(product);

            return Ok(product);
        }


        [HttpGet("custom-object-binding")]
        public IActionResult CustomObjectBinding([FromQuery] string complexData)
        {
            var parts = complexData.Split(':');
            if (parts?.Length == 3)
            {
                var product = new Product
                {
                    Name = parts[0],
                    Description = parts[1],
                    Price = decimal.TryParse(parts[2], out decimal price) ? price : 0
                };

                return Ok(product);
            }

            return BadRequest("Invalid complexData format. Expected format: Name:Category:Price");
        }

        #endregion

    }
}
