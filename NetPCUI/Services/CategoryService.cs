using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetPCUI.Models;

namespace NetPCUI.Services
{
    public class CategoryService
    {
        private readonly HttpClient _httpClient;

        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //public async Task<List<Category>> GetCategoriesAsync()
        //{
        //    return await _httpClient.GetFromJsonAsync<List<Category>>("api/categories") ?? new List<Category>();
        //}

        public async Task<List<Subcategory>> GetSubcategoriesAsync(int categoryId)
        {
            return await _httpClient.GetFromJsonAsync<List<Subcategory>>($"api/categories/{categoryId}/subcategories") ?? new List<Subcategory>();
        }
    }
}