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

    public async Task<bool> DeleteContactAsync(int id)
    {
        try
        {
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

    public async Task UpdateContactAsync(ContactDto contact)
    {
        var response = await _http.PutAsJsonAsync($"api/contacts/{contact.Id}", contact);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Nie udało się zaktualizować kontaktu.");
        }
    }

    public async Task<bool> AddContactAsync(ContactCreateDto newContact, string token)
    {
        try
        {
            _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _http.PostAsJsonAsync("api/Contacts", newContact);

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd przy dodawaniu kontaktu: {ex.Message}");
            return false;
        }
    }
}

