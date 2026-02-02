using EmployeeAdminPortal.Models.EcommerceModel;

namespace EmployeeAdminPortal.Services
{
    public class ProductService : IProductService
    {
        #region Methods

        public IEnumerable<Product> GetAllProducts()
        {
            return _products;
        }

        public Product? GetProductById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public void InsertProduct(Product product)
        {
            _products.Add(product);
        }

        public IEnumerable<Product> SearchProducts(string? category, decimal? minPrice, decimal? maxPrice)
        {
            var query = _products.AsQueryable();
            //if (!string.IsNullOrEmpty(category))
            //{
            //    query = query.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
            //}

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            return query.ToList();
        }

        public bool UpdateProductPrice(int id, decimal discountPercentage)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                var amount = product.Price * discountPercentage / 100;
                product.Price -= amount;
                return true;
            }

            return false;
        }

        #endregion

        #region Utilities

        private readonly List<Product> _products = new()
        {
            new Product { Id = 1, Name = "Laptop", Price = 65000, SKU = "Electronics"},
            new Product { Id = 2, Name = "Headphones", Price = 2500, SKU = "Audio" },
            new Product { Id = 3, Name = "Smartwatch", Price = 12000, SKU = "Wearables" },
            new Product { Id = 4, Name = "Keyboard", Price = 1500, SKU = "Accessories" }
        };

        #endregion
    }
}
