using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using NetPCUI.Models;

public class ContactService
{
    private readonly HttpClient _http;

    public ContactService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<ContactDto>> GetContactsAsync()
    {
        return await _http.GetFromJsonAsync<List<ContactDto>>("api/Contacts");
    }

    public async Task<ContactDto> GetContactByIdAsync(int id)
    {
        try
        {
            var contact = await _http.GetFromJsonAsync<ContactDto>($"api/Contacts/{id}");
            return contact;
        }
        catch (Exception ex) 
        {
            Console.WriteLine($"Błąd podczas pobierania kontaktu: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> DeleteContactAsync(int id, string token)
    {
        try
        {
            _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _http.DeleteAsync($"api/Contacts/{id}");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                Console.WriteLine($"Nie udało się usunąć kontaktu. Status: {response.StatusCode}");
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd podczas usuwania kontaktu: {ex.Message}");
            return false;
        }
    }

    public async Task<HttpResponseMessage> UpdateContactAsync(ContactUpdateDto contact, string token)
    {
        try
        {
            _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _http.PutAsJsonAsync($"api/Contacts/{contact.Id}", contact);
            if (response.IsSuccessStatusCode)
            {
                return response;
            }
            else
            {
                Console.WriteLine($"Nie udało się zaktualizować kontaktu. Status: {response.StatusCode}");
                return response;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd przy aktualizacji kontaktu: {ex.Message}");
            return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
        }
    }

    public async Task<(bool success, string errorMsg, Dictionary<string, List<string>> validationErrors)> AddContactAsync(ContactCreateDto newContact, string token)
    {
        try
        {
            _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _http.PostAsJsonAsync("api/Contacts", newContact);

            if (!response.IsSuccessStatusCode)
            {
                // Zwracamy błędy walidacji w postaci słownika
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var validationResponse = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();
                    return (false, null, validationResponse?.Errors);
                }
                else
                {
                    return (false, "Nie udało się dodać kontaktu.", null);
                }
            }

            return (true, null, null);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd przy dodawaniu kontaktu: {ex.Message}");
            return (false, "Wystąpił błąd przy dodawaniu kontaktu.", null);
        }
    }
}

