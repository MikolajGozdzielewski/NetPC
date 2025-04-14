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
builder.Services.AddScoped<AuthService>();


builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5001") });

builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();