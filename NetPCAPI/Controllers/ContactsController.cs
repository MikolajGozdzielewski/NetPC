using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetPCAPI.Data;
using NetPCAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetPCAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ContactsController(AppDbContext context)
        {
            _context = context;
        }

        //[Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ContactDto>> GetContact(int id)
        {
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
                SubcategoryName = contact.Subcategory?.Name
            };
            return Ok(contactDto);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactDto>>> GetContacts()
        {
            try
            {

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
                        SubcategoryName = c.Subcategory.Name
                    })
                    .ToListAsync();


                return Ok(contacts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Błąd pobierania danych: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Contact>> AddContact([FromBody] ContactDto contactDto)
        {
            // Sprawdzenie poprawności modelu
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Sprawdzenie unikalności emaila
            if (_context.Contacts.Any(c => c.Email == contactDto.Email))
            {
                return BadRequest("Podany adres email jest już zarejestrowany.");
            }

            // Pobranie nazwy kategorii i podkategorii z bazy danych (jeśli są wymagane)
            var category = await _context.Categories.FindAsync(contactDto.CategoryId);
            if (category == null)
            {
                return BadRequest("Nie znaleziono podanej kategorii.");
            }

            string subcategoryName = null;
            if (contactDto.SubcategoryId > 0)
            {
                var subcategory = await _context.Subcategories.FindAsync(contactDto.SubcategoryId);
                if (subcategory == null)
                {
                    return BadRequest("Nie znaleziono podanej podkategorii.");
                }
                subcategoryName = subcategory.Name;
            }

            // Utworzenie nowego obiektu kontaktu
            var contact = new Contact
            {
                FirstName = contactDto.FirstName,
                LastName = contactDto.LastName,
                Email = contactDto.Email,
                PhoneNumber = contactDto.PhoneNumber,
                BirthDate = contactDto.BirthDate,
                CategoryId = contactDto.CategoryId,
                SubcategoryId = contactDto.SubcategoryId,
                Password = BCrypt.Net.BCrypt.HashPassword(contactDto.Password), // Haszowanie hasła
                Category = category,
                Subcategory = subcategoryName != null ? await _context.Subcategories.FindAsync(contactDto.SubcategoryId) : null
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

        //[Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact(int id, [FromBody] ContactDto contactDto)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound("Nie znaleziono kontaktu.");
            }

            contact.FirstName = contactDto.FirstName;
            contact.LastName = contactDto.LastName;
            contact.Email = contactDto.Email;
            contact.PhoneNumber = contactDto.PhoneNumber;
            contact.BirthDate = contactDto.BirthDate;
            contact.CategoryId = contactDto.CategoryId;
            contact.SubcategoryId = contactDto.SubcategoryId;

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

        //[Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound("Nie znaleziono kontaktu.");
            }

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