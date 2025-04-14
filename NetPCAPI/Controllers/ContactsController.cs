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
        public async Task<ActionResult<Contact>> AddContact([FromBody] ContactCreateDto contactCreateDto)
        {
            // Sprawdzenie poprawności modelu
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Sprawdzenie unikalności emaila
            if (_context.Contacts.Any(c => c.Email == contactCreateDto.Email))
            {
                return BadRequest("Podany adres email jest już zarejestrowany.");
            }

            // Pobranie nazwy kategorii i podkategorii z bazy danych (jeśli są wymagane)
            var category = await _context.Categories.FindAsync(contactCreateDto.CategoryId);
            if (category == null)
            {
                return BadRequest("Nie znaleziono podanej kategorii.");
            }

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
                Password = BCrypt.Net.BCrypt.HashPassword(contactCreateDto.Password), // Haszowanie hasła
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

        //[Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact(int id, [FromBody] ContactUpdateDto contactUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound("Nie znaleziono kontaktu.");
            }

            if (_context.Contacts.Any(c => c.Email == contactUpdateDto.Email && c.Id != id))
            {
                return BadRequest("Podany adres email jest już zarejestrowany.");
            }

            if (!string.IsNullOrEmpty(contactUpdateDto.FirstName))
                contact.FirstName = contactUpdateDto.FirstName;

            if (!string.IsNullOrEmpty(contactUpdateDto.LastName))
                contact.LastName = contactUpdateDto.LastName;

            if (!string.IsNullOrEmpty(contactUpdateDto.Email))
                contact.Email = contactUpdateDto.Email;

            if (!string.IsNullOrEmpty(contactUpdateDto.PhoneNumber))
                contact.PhoneNumber = contactUpdateDto.PhoneNumber;

            if (contactUpdateDto.BirthDate.HasValue)
                contact.BirthDate = contactUpdateDto.BirthDate;

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