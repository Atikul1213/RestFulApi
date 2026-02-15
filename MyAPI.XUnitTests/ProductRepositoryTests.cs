using EmployeeAdminPortal.Data;
using EmployeeAdminPortal.Models.EcommerceModel;
using EmployeeAdminPortal.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MyAPI.XUnitTests
{
    public class ProductRepositoryTests
    {
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;

            var context = new ApplicationDbContext(options);

            new Product { Id = 1, Name = "Test Laptop", Price = 60000m, StockQuantity = 20 },       // Product 1
                new Product { Id = 2, Name = "Test Smartphone", Price = 25000m, StockQuantity = 50 }     // Product 2
            );

            context.SaveChanges();
            return context;
        }

        [Fact]
        public async Task GetByIdAsync_ProductExists_ReturnsProduct()
        {
            var context = GetInMemoryDbContext();

            var repository = new ProductRepository(context);

            var product = await repository.GetByIdAsync(1);

            Assert.NotNull(product);
            Assert.Equal("Test Laptop", product.Name);
            Assert.Equal(6000m, product.Price);
        }

        [Fact]
        public async Task GetByIdAsync_ProductDoesNotExist_ReturnsNull()
        {
            var context = GetInMemoryDbContext();
            var repository = new ProductRepository(context);
            var product = await repository.GetByIdAsync(999);

            Assert.Null(product);
        }

        [Fact]
        public async Task AddAsync_ProductIsAdded_ProductExistsInDb()
        {
            var context = GetInMemoryDbContext();
            var repository = new ProductRepository(context);
            var newProduct = new Product
            {
                Id = 3,
                Name = "Test Tablet",
                Price = 15000m,
                StockQuantity = 30
            };

            await repository.AddAsync(newProduct);
            await repository.SaveChangesAsync();

            var addedProduct = await repository.GetByIdAsync(3);
            Assert.NotNull(addedProduct);
            Assert.Equal("Test Tablet", addedProduct.Name);
        }


        [Fact]
        public async Task SaveChangesAsync_ModificationData_DataIsPersisted()
        {
            var context = GetInMemoryDbContext();
            var repository = new ProductRepository(context);

            var product = await repository.GetByIdAsync(1);

            Assert.NotNull(product);
            product.StockQuantity = 10;

            await repository.SaveChangesAsync();

            var updatedProduct = await repository.GetByIdAsync(1);

            Assert.NotNull(updatedProduct);
            Assert.Equal(10, updatedProduct.StockQuantity);
        }
    }
}
