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

    
        //[HttpGet("test-database")]
        //public IActionResult TestDatabase()
        //{
        //    var isConnected = _context.Database.CanConnect();
        //    if (isConnected)
        //    {
        //        return Ok("Baza danych działa poprawnie!");
        //    }
        //    return StatusCode(500, "Nie można połączyć się z bazą danych.");
        //}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> GetContacts()
        {
            try
            {
                //var contacts = await _context.Contacts
                //    .Include(c => c.Category)
                //    .Include(c => c.Subcategory)
                //    .ToListAsync();

                var contacts = await _context.Contacts.ToListAsync();


                return Ok(contacts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Błąd pobierania danych: {ex.Message}");
            }
        }

      
        [HttpPost]
        public async Task<ActionResult<Contact>> AddContact([FromBody] Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //try
            //{
            //    _context.Contacts.Add(contact);
            //    await _context.SaveChangesAsync();
            //    return CreatedAtAction(nameof(GetContacts), new { id = contact.Id }, contact);
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(500, $"Wystąpił błąd podczas dodawania kontaktu: {ex.Message}");
            //}
            try
            {
                _context.Contacts.Add(contact);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetContacts), new { id = contact.Id }, contact);
            }
            catch (Exception ex)
            {
                return StatusCode(500,$"Błąd zapisu: {ex.InnerException?.Message}");
            }
        }
    }
}