using Lagersystem.Blazor.Services.Abstractions;
using Lagersystem.Blazor.Services.InMemory;
using Lagersystem.Blazor.State;

namespace Lagersystem.Blazor.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IProductService, InMemoryProductService>();
        services.AddScoped<IOrderService, InMemoryOrderService>();

        services.AddScoped<ProductState>();
        services.AddScoped<OrderState>();
        services.AddScoped<CustomerState>();

        return services;
    }
}