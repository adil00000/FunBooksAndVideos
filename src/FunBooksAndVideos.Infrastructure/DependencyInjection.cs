using FunBooksAndVideos.Application.Abstractions.Persistence;
using FunBooksAndVideos.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace FunBooksAndVideos.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Registers in-memory repositories. Swap these registrations for EF Core / Dapper
    /// implementations without touching the Application or API layers.
    /// </summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IProductRepository>(_ => new InMemoryProductRepository(SeedData.Products()));
        services.AddSingleton<ICustomerRepository>(_ => new InMemoryCustomerRepository(SeedData.Customers()));
        services.AddSingleton<IPurchaseOrderRepository>(_ => new InMemoryPurchaseOrderRepository());
        services.AddSingleton<IShippingSlipRepository, InMemoryShippingSlipRepository>();

        return services;
    }
}
