using EmployeeAdminPortal.Entities;
using EmployeeAdminPortal.Models.EcommerceModel;
using EmployeeAdminPortal.Models.JwtToken.Model;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAdminPortal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<ProductCategory> ProductCategories { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Address> Addresses { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;
        public DbSet<Payment> Payments { get; set; } = null!;
        public DbSet<OrderHistory> OrderHistories { get; set; } = null!;
        public DbSet<FailureLog> FailureLogs => Set<FailureLog>();
        public DbSet<Employee> Employees { get; set; }



        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<SigningKey> SigningKeys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<Role>().HasData(
               new Role { Id = 1, Name = "Admin", Description = "Admin Role" },
                new Role { Id = 2, Name = "Editor", Description = " Editor Role" },
                new Role { Id = 3, Name = "User", Description = "User Role" }
                );

            modelBuilder.Entity<Client>().HasData(
                new Client()
                {
                    Id = 1,
                    ClientId = "Client1",
                    Name = "Client Application 1",
                    ClientURL = "https://client1.com"
                },
                new Client()
                {
                    Id = 2,
                    ClientId = "Client2",
                    Name = "Client Application 2",
                    ClientURL = "https://client2.com"
                }
             );

            modelBuilder.Entity<Order>()
                .Property(o => o.OrderStatus)
                .HasConversion<string>()
                .HasMaxLength(30);

            modelBuilder.Entity<Payment>()
                .Property(p => p.PaymentStatus)
                .HasConversion<string>()
                .HasMaxLength(30);

            modelBuilder.Entity<OrderHistory>()
                .Property(h => h.NewStatus)
                .HasConversion<string>()
                .HasMaxLength(30);

            modelBuilder.Entity<OrderHistory>()
                .Property(h => h.OldStatus)
                .HasConversion<string>()
                .HasMaxLength(30);

            ConfigureProductCategoryMapping(modelBuilder);
        }


        private void ConfigureProductCategoryMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductCategory>()
                .HasKey(pc => new { pc.ProductId, pc.CategoryId });

            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Product)
                .WithMany(p => p.ProductCategories)
                .HasForeignKey(pc => pc.ProductId);

            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Category)
                .WithMany(c => c.ProductCategories)
                .HasForeignKey(pc => pc.CategoryId);
        }
    }
}
