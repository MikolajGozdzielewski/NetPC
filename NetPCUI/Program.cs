using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NetPCUI;



var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

// Dodanie usług, które będą dostępne
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<ContactService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<AuthService>();

// Dodanie domyślnego adresu dla zapytań(backend)
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5001") });

// Dodanie autoryzacji
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();