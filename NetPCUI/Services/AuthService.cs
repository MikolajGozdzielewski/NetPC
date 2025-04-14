using Microsoft.JSInterop;

public class AuthService
{
    private readonly IJSRuntime _jsRuntime;

    public AuthService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<bool> IsLoggedInAsync()
    {
        var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "jwtToken");
        return !string.IsNullOrEmpty(token);
    }

    public async Task LogoutAsync()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "jwtToken");
    }
}
