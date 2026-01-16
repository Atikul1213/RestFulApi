using EmployeeAdminPortal.Models.JwtToken.DTO;
using EmployeeAdminPortal.Models.JwtToken.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeAdminPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private readonly List<User> _user = new List<User>()
        {
            new User(){
                Id =1, Email = "atikuldpi@gmail.com", FirstName = "Atikul",
                LastName = "Islam", Password="atikul123",
            },
            new User()
            {
                Id = 2, Email="ismail@gmail.com", FirstName="Ismail",
                LastName="Hossain", Password="ismail123",
            }
        };

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginDTO loginDto)
        {
            var user = _user.FirstOrDefault(u =>
            u.Email.Equals(loginDto.Email, StringComparison.OrdinalIgnoreCase)
            && u.Password == loginDto.Password);

            if (user == null)
                return Unauthorized("Invalid Credentials");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim("SubscriptionLevel", user.FirstName ?? "Free"),
                new Claim("Department", user.LastName ?? "None")
            };


            var secretKey = _configuration.GetValue<String>("JwtSettings:SecretKey") ??
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTUxNjIzOTAyMn0.KMUFsIDTnFmyG3nMiGM6H9FNFUROf3wh7SmqJp-QV30";

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { Token = tokenString });
        }
    }
}
