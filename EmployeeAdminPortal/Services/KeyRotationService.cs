
using EmployeeAdminPortal.Data;
using EmployeeAdminPortal.Models.JwtToken.Model;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace EmployeeAdminPortal.Services
{
    public class KeyRotationService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _rotationInterval = TimeSpan.FromDays(7);

        public KeyRotationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await RotateKeysAsync();
                await Task.Delay(_rotationInterval, stoppingToken);
            }
        }

        private async Task RotateKeysAsync()
        {
            using var scope = _serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var activeKey = await context.SigningKeys.FirstOrDefaultAsync(k => k.IsActive);

            if (activeKey == null || activeKey.ExpireAt <= DateTime.UtcNow.AddDays(10))
            {
                if (activeKey != null)
                {
                    activeKey.IsActive = false;
                    context.SigningKeys.Update(activeKey);
                }

                // Generate a new RSA key pair.
                using var rsa = RSA.Create(2048);
                // Export the private key as a Base64-encoded string.
                var privateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());
                // Export the public key as a Base64-encoded string.
                var publicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey());
                // Generate a unique identifier for the new key.
                var newKeyId = Guid.NewGuid().ToString();

                // Create a new SigningKey entity with the new RSA key details.
                var newKey = new SigningKey
                {
                    KeyId = newKeyId,
                    PrivateKey = privateKey,
                    PublicKey = publicKey,
                    IsActive = true,
                    CreateAt = DateTime.UtcNow,
                    ExpireAt = DateTime.UtcNow.AddYears(1)
                };

                await context.SigningKeys.AddAsync(newKey);

                await context.SaveChangesAsync();
            }
        }
    }
}
