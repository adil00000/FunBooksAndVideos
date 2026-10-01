using FunBooksAndVideos.Application.Abstractions.Shipping;
using FunBooksAndVideos.Application.Processing;
using FunBooksAndVideos.Application.Processing.Rules;
using FunBooksAndVideos.Application.Services;
using FunBooksAndVideos.Application.Shipping;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FunBooksAndVideos.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.TryAddSingleton(TimeProvider.System);

        // Purchase order processor + business rules.
        // Adding a new business rule = one new class + one line here. Nothing else changes.
        services.AddScoped<IPurchaseOrderProcessor, PurchaseOrderProcessor>();
        services.AddScoped<IPurchaseOrderRule, MembershipActivationRule>();
        services.AddScoped<IPurchaseOrderRule, ShippingSlipRule>();

        services.AddSingleton<IShippingSlipFactory, ShippingSlipFactory>();

        // Application services used by the controllers.
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
        services.AddScoped<IShippingSlipService, ShippingSlipService>();

        return services;
    }
}
