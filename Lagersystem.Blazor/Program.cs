using Lagersystem.Blazor;
using Lagersystem.Blazor.Extensions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddScoped(_ => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7289/")
});

builder.Services.AddApplicationServices();
await builder.Build().RunAsync();