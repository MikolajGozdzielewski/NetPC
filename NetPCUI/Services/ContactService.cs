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

    public async Task<List<Contact>> GetContactsAsync()
    {
        return await _http.GetFromJsonAsync<List<Contact>>("api/Contacts");
    }
}

