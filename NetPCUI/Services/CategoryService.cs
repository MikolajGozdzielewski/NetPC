using System.Text.Json;
using NetPCUI.Models;

/**
    * <summary>
    * Serwis zajmuje się obsługą komunikacji z API związanych z kategoriami i podkategoriami.
    * </summary>
*/
public class CategoryService
{
    private readonly HttpClient _http;

    public CategoryService(HttpClient http)
    {
        _http = http;
    }

    /**
    * <summary>
    * Funkcja służy do wyciągnięcia z bazy danych kategorii, przez API.
    * </summary>
*/
    public async Task<List<CategoryDto>> GetCategoriesAsync()
    {
        var response = await _http.GetAsync("api/categories");
        var categories = await DeserializeResponseAsync<CategoryDto>(response);
        return categories;
    }

    /**
    * <summary>
    * Funkcja służy do wyciągnięcia z bazy danych podkategorii, dla wybranej kategorii, przez API.
    * </summary>
*/
    public async Task<List<SubcategoryDto>> GetSubcategoriesByCategoryIdAsync(int categoryId)
    {
        var response = await _http.GetAsync($"api/categories/{categoryId}/subcategories");
        var subcategories = await DeserializeResponseAsync<SubcategoryDto>(response);
        return subcategories;
    }

    /**
    * <summary>
    * Funkcja służy do zamiany formatu JSON na listę, dla formularza.
    * </summary>
*/
    private async Task<List<T>> DeserializeResponseAsync<T>(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<T>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new();
        }

        return new();
    }
}
