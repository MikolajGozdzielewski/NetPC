namespace NetPCAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Microsoft.IdentityModel.Tokens;
    using NetPCAPI.Data;
    using NetPCAPI.Models;

    /**
     * <summary>
     * Kontroler zarządzający logowaniem się użytkowników do aplikacji.
     * Obsługuje logowanie i generowanie tokenów JWT.
     * JWT zostało wybrane przez prostą implementację oraz ograniczoną ilość czasu na wykonanie zadania, co uniemożliwiło dokonanie dokładniejszego researchu.
     * </summary>
    */
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        /*
         *<summary>
         *Konstruktor kontrolera, wstrzykuje dane z bazy oraz konfigurację tokena z pliku appsettings.json, potrzebną przy jego tworzeniu.
         *</summary>
        */
        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context; 
            _configuration = configuration;
        }

        /*
         * <summary>
         * Metoda umożliwiająca logowanie. Wykorzystuje funkcję haszującą BCrypt do porównania haseł, ponieważ pod taką formą hasła są przechowywane w bazie.
         * </summary>
         * <returns>Zwraca token JWT jeśli logowanie odbyło się poprawnie, w innym wypadku zwraca informację o błędzie.</returns>
        */ 
        [HttpPost("login")]
        public async Task<IActionResult> Login(ContactLoginDto request)
        {
            // Pobranie danych kontaktu o podanym emailu
            var contact = _context.Contacts.FirstOrDefault(c => c.Email == request.Email);

            // Błąd przy nieistniejącym emailu
            if (contact == null)
            {
                return Unauthorized("Nieprawidłowy email.");
            }

            // Weryfikacja hasła
            if (!BCrypt.Net.BCrypt.Verify(request.Password, contact.Password))
            {
                return Unauthorized("Nieprawidłowe hasło.");
            }

            // Generowanie tokena
            var token = GenerateJwtToken(contact);

            return Ok(new { Token = token });
        }

        /*
         * <summary>
         * Funkcja generująca Token JWT.
         * </summary>
         * <remarks>
         * Funkcja generująca token została zainspirowana artykułem ze strony:
         * https://dotnetfullstackdev.medium.com/jwt-token-authentication-in-c-a-beginners-guide-with-code-snippets-7545f4c7c597
         * Rozdział 4: Generating JWT Tokens
         * Zmiany w kodzie to dodanie rozszerzenia claims, które przechowuje dane, które zostaną umieszczone w tokenie oraz wykorzystanie danych z wcześniejszego konstruktowa.
         * </remarks>
        */
        private string GenerateJwtToken(Contact contact)
        {
            //Stworzenie rozszerzeń calims, przechowujących dane, które zostaną umieszczone w tokenie.
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