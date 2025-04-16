using Microsoft.JSInterop;

/*
* <summary>
* Serwis odpowiedzialny za autoryzację użytkownika.
* Wykorzystałem schowek lokalny, pomimo słabych zabezpieczeń z powodu problemów z wykorzystaniem ciasteczek, a przez ograniczoną ilość czasu nie mogłem zagłębić się bardziej w szukanie błędów.
* </summary>
*/
public class AuthService
{
    private readonly IJSRuntime _jsRuntime;

    /**
     * <summary>
     * Inicjalizacja umożliwiająca wywoływanie JS z poziomu C#, potrzebna do wykorzystania schowku lokalnego.
     * </summary>
    */
    public AuthService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public event Action? OnAuthStateChanged;

    /**
     * <summary>
     *  Funkcja sprawdza, czy użytkownik jest zalogowany, czy ma token.
     * </summary>
    */
    public async Task<bool> IsLoggedInAsync()
    {
        var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "jwtToken");
        return !string.IsNullOrEmpty(token);
    }

    /**
     * <summary>
     *  Funkcja zapisuje token w schowku lokalnym/loguje użytkownika.
     * </summary>
    */
    public async Task LoginAsync(string token)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "jwtToken", token);
        OnAuthStateChanged?.Invoke();
    }

    /**
     * <summary>
     *  Funkcja wylogowywuje użytkownika
     * </summary>
    */
    public async Task LogoutAsync()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "jwtToken");
        OnAuthStateChanged?.Invoke();
    }
}
