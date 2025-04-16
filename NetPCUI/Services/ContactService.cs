using System.Net.Http.Json;
using NetPCUI.Models;

/**
    * <summary>
    * Serwis zajmuje się obsługą komunikacji z API związanych z klientami.
    * </summary>
*/
public class ContactService
{
    private readonly HttpClient _http;

    public ContactService(HttpClient http)
    {
        _http = http;
    }

    /**
    * <summary>
    * Funkcja pobiera z backendu listę kontaktów
    * </summary>
*/
    public async Task<List<ContactDto>> GetContactsAsync()
    {
        return await _http.GetFromJsonAsync<List<ContactDto>>("api/Contacts");
    }

    /**
    * <summary>
    * Funkcja pobiera z backendu dane konkretnego kontaktu.
    * </summary>
*/
    public async Task<ContactDto> GetContactByIdAsync(int id)
    {
        try
        {
            var contact = await _http.GetFromJsonAsync<ContactDto>($"api/Contacts/{id}");
            return contact;
        }
        catch (Exception ex) 
        {
            return null;
        }
    }

    /**
    * <summary>
    * Funkcja usuwa wybrany kontakt z bazy.
    * </summary>
*/
    public async Task<bool> DeleteContactAsync(int id, string token)
    {
        try
        {
            // Usunięcie z dodaną autoryzacją
            _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _http.DeleteAsync($"api/Contacts/{id}");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    /**
    * <summary>
    * Funkcja edytuje wybrany kontakt z bazy.
    * </summary>
*/
    public async Task<HttpResponseMessage> UpdateContactAsync(ContactUpdateDto contact, string token)
    {
        try
        {
            // Edycja z dodaną autoryzacją.
            _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _http.PutAsJsonAsync($"api/Contacts/{contact.Id}", contact);
            if (response.IsSuccessStatusCode)
            {
                return response;
            }
            else
            {
                return response;
            }
        }
        catch (Exception ex)
        {
            return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
        }
    }

    /**
    * <summary>
    * Funkcja dodaje kontakt do bazy.
    * </summary>
*/
    public async Task<(bool success, string errorMsg, Dictionary<string, List<string>> validationErrors)> AddContactAsync(ContactCreateDto newContact, string token)
    {
        try
        {
            // Dodanie kontaktu z dodaną autoryzacją
            _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _http.PostAsJsonAsync("api/Contacts", newContact);

            if (!response.IsSuccessStatusCode)
            {
                // Pobranie błędu wysłanego z backendu
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
            return (false, "Wystąpił błąd przy dodawaniu kontaktu.", null);
        }
    }
}

