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
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {
            try
            {
                var categories = await _context.Categories
                    .Include(c => c.Subcategories) // Pobierz relacje z Subcategories
                    .Select(c => new CategoryDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Subcategories = c.Subcategories.Select(s => new SubcategoryDto
                        {
                            Id = s.Id,
                            Name = s.Name
                        }).ToList() // Zamień encje Subcategory na DTO
                    })
                    .ToListAsync(); // Pobierz dane jako lista DTO

                return Ok(categories); // Zwrot listy DTO
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Błąd pobierania danych: {ex.Message}");
            }
        }

        [HttpGet("{id}/subcategories")]
        public async Task<ActionResult<IEnumerable<Subcategory>>> GetSubcategories(int id)
        {
            var subcategories = await _context.Subcategories.Where(s => s.CategoryId == id).ToListAsync();
            return Ok(subcategories);
        }
    }
}