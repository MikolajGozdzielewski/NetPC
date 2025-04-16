using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetPCAPI.Data;
using NetPCAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetPCAPI.Controllers
{

    /**
     * <summary>
     * Kontroler zarządzający wysyłaniem, dodawaniem i edytowaniem danych z tabeli Contacts.
     * </summary>
    */
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly AppDbContext _context;

        /*
         * <summary>
         * Konstruktor kontrolera, wstrzykuje dane z bazy.
         * </summary>
        */
        public ContactsController(AppDbContext context)
        {
            _context = context;
        }

        /*
         * <summary>
         * Metoda wyciągająca z bazy informację o konkretnym kontakcie i rzutuje je z klasy Contact na ContactDto.
         * </summary>
         * <returns>
         * Jeśli nie pojawił się błąd wysyła klasy ContactDto, jeśli doszło do błędu wysyła informację o błędzie. 
         * Potencjalne błędy mogą pojawić się przy zmianach lub usunięciu klas Contact i ContactDto.
         * </returns>
        */
        [HttpGet("{id}")]
        public async Task<ActionResult<ContactDto>> GetContact(int id)
        {
            try
            {
                // Mapowanie klasy Contacts na ContactsDto dla podanego id
                var contact = await _context.Contacts
                    .Include(c => c.Category)
                    .Include(c => c.Subcategory)
                    .FirstOrDefaultAsync(c => c.Id == id);
                if (contact == null)
                {
                    return NotFound("Nie znaleziono kontaktu");
                }
                var contactDto = new ContactDto
                {
                    Id = contact.Id,
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    Email = contact.Email,
                    PhoneNumber = contact.PhoneNumber,
                    BirthDate = contact.BirthDate,
                    CategoryId = contact.CategoryId,
                    CategoryName = contact.Category.Name,
                    SubcategoryId = contact.SubcategoryId,
                    SubcategoryName = contact.Subcategory?.Name,
                    AnotherSubcategory = contact.AnotherSubcategory
                };
                return Ok(contactDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Błąd pobierania danych: {ex.Message}");
            }
        }

        /*
         * <summary>
         * Metoda wyciągająca z bazy informację o wszystkich kontaktach i rzutuje je z klasy Contact na ContactDto. Różni się od [HttpGet("{id}")], tym że zwraca listę, a nie pojedyczy obiekt.
         * </summary>
         * <returns>
         * Jeśli nie pojawił się błąd wysyła obiekt o klasie ContactDto, jeśli doszło do błędu wysyła informację o błędzie. 
         * Potencjalne błędy mogą pojawić się przy zmianach lub usunięciu klas Contact i ContactDto.
         * </returns>
        */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactDto>>> GetContacts()
        {
            try
            {
                // Mapowanie klasy Contact na ContactDto, dla wszystkich wierszy w tabeli
                var contacts = await _context.Contacts
                    .Include(c => c.Category)
                    .Include(c => c.Subcategory)
                    .Select(c => new ContactDto
                    {
                        Id = c.Id,
                        FirstName = c.FirstName,
                        LastName = c.LastName,
                        Email = c.Email,
                        PhoneNumber = c.PhoneNumber,
                        BirthDate = c.BirthDate,
                        CategoryId = c.CategoryId,
                        CategoryName = c.Category.Name,
                        SubcategoryId = c.SubcategoryId,
                        SubcategoryName = c.Subcategory.Name,
                        AnotherSubcategory = c.AnotherSubcategory
                    })
                    .ToListAsync();


                return Ok(contacts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Błąd pobierania danych: {ex.Message}");
            }
        }

        /*
         * <summary>
         * Metoda dodaje do bazy nowy kontakt, przyjmuje dane do obiektu o klasie ContactCreateDto, sprawdza unikalność emaila i rzutuje je z klasy ContactCreateDto na Contact.
         * Działa tylko dla zalogowanego urzytkownika.
         * </summary>
         * <returns>
         * Jeśli nie pojawił się błąd wysyła obiekt klasy ContactDto, zrzutowany z Contact, potwierdzający dodanie kontakctu, jeśli doszło do błędu wysyła informację o błędzie. 
         * Potencjalne błędy mogą pojawić się przy nie spełnieniu wymagać klasy, braku unikalności emaila, niepoprawnej kategorii lub podkategorii zmianach i potencjalnych błędach zapisu.
         * </returns>
        */
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Contact>> AddContact([FromBody] ContactCreateDto contactCreateDto)
        {
            if (!ModelState.IsValid)
            {
                // Wysłanie błędu zapisanego w klasie.
                return BadRequest(ModelState); 
            }

            // Sprawdzenie unikalności emaila
            if (_context.Contacts.Any(c => c.Email == contactCreateDto.Email))
            {
                return BadRequest("Podany adres email jest już zarejestrowany.");
            }

            // Sprawdzenie poprawności podanej kategorii
            var category = await _context.Categories.FindAsync(contactCreateDto.CategoryId);
            if (category == null)
            {
                return BadRequest("Nie znaleziono podanej kategorii.");
            }

            // Sprawdzenie poprawności podanej podkategorii
            string subcategoryName = null;
            if (contactCreateDto.SubcategoryId > 0)
            {
                var subcategory = await _context.Subcategories.FindAsync(contactCreateDto.SubcategoryId);
                if (subcategory == null)
                {
                    return BadRequest("Nie znaleziono podanej podkategorii.");
                }
                subcategoryName = subcategory.Name;
            }

            // Utworzenie nowego obiektu kontaktu
            var contact = new Contact
            {
                FirstName = contactCreateDto.FirstName,
                LastName = contactCreateDto.LastName,
                Email = contactCreateDto.Email,
                PhoneNumber = contactCreateDto.PhoneNumber,
                BirthDate = contactCreateDto.BirthDate,
                CategoryId = contactCreateDto.CategoryId,
                SubcategoryId = contactCreateDto.SubcategoryId,
                AnotherSubcategory = contactCreateDto.AnotherSubcategory,
                Password = BCrypt.Net.BCrypt.HashPassword(contactCreateDto.Password), // Haszowanie hasła z wykorzystaniem BCrypt
                Category = category,
                Subcategory = subcategoryName != null ? await _context.Subcategories.FindAsync(contactCreateDto.SubcategoryId) : null
            };

            // Dodanie kontaktu do bazy danych
            _context.Contacts.Add(contact);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Błąd zapisu: {ex.InnerException?.Message}");
            }

            // Zwrócenie odpowiedzi (Created)
            return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, new ContactDto
            {
                Id = contact.Id,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Email = contact.Email,
                PhoneNumber = contact.PhoneNumber,
                BirthDate = contact.BirthDate,
                CategoryId = contact.CategoryId,
                CategoryName = category.Name,
                SubcategoryId = contact.SubcategoryId,
                SubcategoryName = subcategoryName
            });
        }

        /*
         * <summary>
         * Metoda edytuje kontakt, przyjmuje dane do obiektu o klasie ContactUpdateDto, sprawdza unikalność emaila i rzutuje je z klasy ContactCreateDto na Contact.
         * Działa tylko dla zalogowanego urzytkownika.
         * </summary>
         * <returns>
         * Jeśli nie pojawił się błąd nie wysyłana jest informacja o błędzie, jeśli doszło do błędu wysyła informację o nim. 
         * Potencjalne błędy mogą pojawić się przy nie spełnieniu wymagać klasy, braku unikalności emaila, niepoprawnej kategorii lub podkategorii zmianach i potencjalnych błędach zapisu.
         * </returns>
        */
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact(int id, [FromBody] ContactUpdateDto contactUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                // Wysłanie błądu zapisanego w klasie
                return BadRequest(ModelState);
            }

            // Pobranie danych kontaktu po id i sprawdzenie czy istnieje
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound("Nie znaleziono kontaktu.");
            }

            // Sprawdzenie czy nowy mail, nie jest już wpisany innemu kontaktowi
            if (_context.Contacts.Any(c => c.Email == contactUpdateDto.Email && c.Id != id))
            {
                // Inny format odpowiedzi spowodowany jest tym, żeby ta konkretna dobrze się wyświetlała na stronie, resztę można walidować we frontendzie
                return BadRequest(new { Email = new[] { "Podany adres email jest już zarejestrowany." } });
            }

            // Nadpisanie kontaktu i modyfikacja
            if (!string.IsNullOrEmpty(contactUpdateDto.FirstName))
                contact.FirstName = contactUpdateDto.FirstName;

            if (!string.IsNullOrEmpty(contactUpdateDto.LastName))
                contact.LastName = contactUpdateDto.LastName;

            if (!string.IsNullOrEmpty(contactUpdateDto.Email))
                contact.Email = contactUpdateDto.Email;

            if (!string.IsNullOrEmpty(contactUpdateDto.PhoneNumber))
                contact.PhoneNumber = contactUpdateDto.PhoneNumber;

            if (!string.IsNullOrEmpty(contactUpdateDto.AnotherSubcategory))
                contact.AnotherSubcategory = contactUpdateDto.AnotherSubcategory;

            if (contactUpdateDto.BirthDate.HasValue)
                contact.BirthDate = contactUpdateDto.BirthDate;

            // Modyfikacja kategorii kontaktu
            if (contactUpdateDto.CategoryId.HasValue)
            {
                var category = await _context.Categories.FindAsync(contactUpdateDto.CategoryId);
                if (category == null)
                {
                    return BadRequest("Nie znaleziono podanej kategorii.");
                } else
                {
                    contact.CategoryId = contactUpdateDto.CategoryId.Value;
                    contact.Category = category;
                }
            }

            // Modyfikacja podkategorii kontaktu
            if (contactUpdateDto.SubcategoryId.HasValue)
            {
                var subcategory = await _context.Subcategories.FindAsync(contactUpdateDto.SubcategoryId.Value);
                if (subcategory == null)
                {
                    return BadRequest("Nie znaleziono podanej podkategorii.");
                } else
                {
                    contact.SubcategoryId = contactUpdateDto.SubcategoryId.Value;
                    contact.Subcategory = subcategory;
                }
            }

            // Modyfikacja i zapisanie zmian
            _context.Entry(contact).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Błąd zapisu: {ex.InnerException?.Message}");
            }

            return NoContent();
        }

        /*
         * <summary>
         * Metoda usuwa kontakt.
         * Działa tylko dla zalogowanego urzytkownika.
         * </summary>
         * <returns>
         * Jeśli nie pojawił się błąd nie wysyłana jest informacja o błędzie, jeśli doszło do błędu wysyła informację o nim. 
         * Potencjalne błędy mogą pojawić się przy podaniu nieprawidłowego id.
         * </returns>
        */
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            // Pobranie kontaktu i sprawdzenie czy kontakt o takim id istnieje
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound("Nie znaleziono kontaktu.");
            }

            // Usunięcie kontaktu
            _context.Contacts.Remove(contact);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Błąd zapisu: {ex.InnerException?.Message}");
            }
            return NoContent();
        }

    }
}