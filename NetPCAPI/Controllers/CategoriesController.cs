using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetPCAPI.Data;
using NetPCAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetPCAPI.Controllers
{
    /*
     * <summary> 
     * Kontroler zarządzający przesyłem danych z tabel Category i Subcategory z bazy danych.
     * </summary>
    */
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        /*
         * <summary>
         * Konstruktor kontrolera, wstrzykuje dane z bazy.
         * </summary>
        */
        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        /*
         * <summary>
         * Metoda zwraca z bazy informację o kategoriach i rzutuje je z klasy Category na CategoryDto.
         * </summary>
         * <returns>
         * Jeśli nie pojawił się błąd wysyła obiekt o klasie CategoryDto, jeśli doszło do błędu wysyła informację o błędzie. 
         * Potencjalne błędy mogą pojawić się przy zmianach lub usunięciu klas Category i CategoryDto.
         * </returns>
        */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {
            try
            {
                // Mapowanie klasy Category na CategoryDto
                var categories = await _context.Categories
                    .Include(c => c.Subcategories) 
                    .Select(c => new CategoryDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Subcategories = c.Subcategories.Select(s => new SubcategoryDto
                        {
                            Id = s.Id,
                            Name = s.Name
                        }).ToList() 
                    })
                    .ToListAsync(); 

                return Ok(categories); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Błąd pobierania danych: {ex.Message}");
            }
        }

        /*
         * <summary>
         * Metoda zwraca z bazy informację o podkategoriach dla konkretnej kategorii i rzutuje je z klasy Subcategory na SubcategoryDto.
         * </summary>
         * <returns>
         * Jeśli nie pojawił się błąd wysyła obiekt o klasie SubcategoryDto, jeśli doszło do błędu wysyła informację o błędzie. 
         * Potencjalne błędy mogą pojawić się przy zmianach lub usunięciu klas Subcategory i SubcategoryDto oraz przy podaniu id niestniejącej kategorii.
         * </returns>
        */
        [HttpGet("{id}/subcategories")]
        public async Task<ActionResult<IEnumerable<Subcategory>>> GetSubcategories(int id)
        {
            try
            {
                // Mapowanie klasy Subcategory na SubcategoryDto
                var subcategories = await _context.Subcategories
                .Where(s => s.CategoryId == id)
                .Select(s => new SubcategoryDto
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .ToListAsync();
                return Ok(subcategories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Wystąpił błąd podczas pobierania podkategorii: {ex.Message}");
            }
        }
    }
}