using Lagersystem.Blazor.API.Clients;
using Lagersystem.Blazor.Services.Abstractions;
using Lagersystem.Blazor.Services.Api;
using Lagersystem.Blazor.State;

namespace Lagersystem.Blazor.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ApiClient>();

        services.AddScoped<IProductService, ProductApiService>();
        services.AddScoped<ProductState>();

        services.AddScoped<IOrderService, OrderApiService>();
        services.AddScoped<OrderState>();

        services.AddScoped<ICustomerService, CustomerApiService>();
        services.AddScoped<CustomerState>();

        return services;
    }
}