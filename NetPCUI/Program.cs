//using Blazored.LocalStorage;
//using Microsoft.AspNetCore.Components.Web;
//using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
//using NetPCUI;
//using System.Net.Http;



//var builder = WebAssemblyHostBuilder.CreateDefault(args);
//builder.RootComponents.Add<App>("#app");

//builder.Services.AddBlazoredLocalStorage();

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5001") });

//builder.Services.AddScoped<ContactService>();

//await builder.Build().RunAsync();








using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using NetPCUI;
using System.Net.Http;
using NetPCUI.Services;



var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<ContactService>();
builder.Services.AddScoped<CategoryService>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5001") });

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();
builder.Services.AddScoped<JwtAuthenticationStateProvider>();

await builder.Build().RunAsync();












//using Blazored.LocalStorage;
//using Microsoft.AspNetCore.Components.Web;
//using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
//using Microsoft.AspNetCore.Components.Authorization;
//using NetPCUI;
//using System.Net.Http;
//using NetPCUI.Services;



//var builder = WebAssemblyHostBuilder.CreateDefault(args);
//builder.RootComponents.Add<App>("#app");

//// Rejestracja usług
//builder.Services.AddBlazoredLocalStorage();
//builder.Services.AddScoped<ContactService>();
//builder.Services.AddScoped<CategoryService>();

//// HttpClient z automatycznym tokenem JWT
//builder.Services.AddScoped<HttpClient>(sp =>
//{
//    var localStorage = sp.GetRequiredService<ILocalStorageService>();
//    var client = new HttpClient
//    {
//        BaseAddress = new Uri("https://localhost:5001")
//    };

//    Task.Run(async () =>
//    {
//        var token = await localStorage.GetItemAsync<string>("authToken");
//        if (!string.IsNullOrWhiteSpace(token))
//        {
//            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
//        }
//    }).Wait();

//    return client;
//});

//// Konfiguracja autoryzacji
//builder.Services.AddAuthorizationCore();
//builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();
//builder.Services.AddScoped<JwtAuthenticationStateProvider>();

//await builder.Build().RunAsync();


