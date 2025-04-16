using Microsoft.JSInterop;

public class AuthService
{
    private readonly IJSRuntime _jsRuntime;

    public AuthService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public event Action? OnAuthStateChanged;

    public async Task<bool> IsLoggedInAsync()
    {
        var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "jwtToken");
        return !string.IsNullOrEmpty(token);
    }

    public async Task LoginAsync(string token)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "jwtToken", token);
        OnAuthStateChanged?.Invoke();
    }

    public async Task LogoutAsync()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "jwtToken");
        OnAuthStateChanged?.Invoke();
    }
}
