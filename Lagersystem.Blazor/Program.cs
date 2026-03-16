using Lagersystem.Blazor;
using Lagersystem.Blazor.Extensions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


// TODO:
// Skift base address til den adresse hvor API faktisk kører.
// Eksempel:
//     BaseAddress = new Uri("https://localhost:5242/")
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddApplicationServices();
await builder.Build().RunAsync();