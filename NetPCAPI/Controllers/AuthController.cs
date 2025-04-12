namespace NetPCAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Microsoft.IdentityModel.Tokens;
    using NetPCAPI.Data;
    using NetPCAPI.Models;

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(ContactLoginDto request)
        {
            var contact = _context.Contacts.FirstOrDefault(c => c.Email == request.Email && c.Password == request.Password);
            if (contact == null)
            {
                return Unauthorized("Nieprawidłowy email lub hasło.");
            }

            var token = GenerateJwtToken(contact);
            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(Contact contact)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, contact.Email),
                new Claim("ContactId", contact.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])); 
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); 

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}