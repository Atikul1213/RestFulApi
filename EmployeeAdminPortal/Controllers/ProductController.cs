using EmployeeAdminPortal.Data;
using EmployeeAdminPortal.Models.EcommerceModel;
using EmployeeAdminPortal.Models.EcommerceModel.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAdminPortal.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Endpoint: GET /api/products/GetProducts?name={name}&category={category}&minPrice={minPrice}&maxPrice={maxPrice}
        [HttpGet("GetProducts")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(
            [FromQuery] string? name, [FromQuery] string? category,
            [FromQuery] decimal? minPrice, decimal? maxPrice)
        {

            var query = _dbContext.Products.AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(p => p.Name.Contains(name));

            if (!string.IsNullOrEmpty(category))
                query = query.Where(p => p.Category.Contains(category));

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
                Category = productCreateDto.Category,
                Price = productCreateDto.Price,
                Stock = productCreateDto.Stock
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

    }
}
