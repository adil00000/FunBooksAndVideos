using FunBooksAndVideos.Application;
using FunBooksAndVideos.Application.Abstractions.Persistence;
using FunBooksAndVideos.Application.Contracts.Requests;
using FunBooksAndVideos.Application.Exceptions;
using FunBooksAndVideos.Application.Services;
using FunBooksAndVideos.Domain.Enums;
using FunBooksAndVideos.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace FunBooksAndVideos.Tests.ApplicationLayer;

/// <summary>Exercises the real IoC wiring (Application + Infrastructure) without HTTP.</summary>
public class PurchaseOrderServiceTests
{
    private static ServiceProvider BuildProvider() =>
        new ServiceCollection()
            .AddLogging()
            .AddApplication()
            .AddInfrastructure()
            .BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true, ValidateOnBuild = true });

    private static CreatePurchaseOrderRequest ExampleRequest() => new()
    {
        CustomerId = 4567890,
        Items = new()
        {
            new PurchaseOrderLineRequest { ProductId = 1 },
            new PurchaseOrderLineRequest { ProductId = 2 },
            new PurchaseOrderLineRequest { ProductId = 3 }
        }
    };

    [Fact]
    public async Task Example_order_is_created_and_both_rules_apply()
    {
        using var provider = BuildProvider();
        using var scope = provider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IPurchaseOrderService>();

        var result = await service.CreateAsync(ExampleRequest());

        Assert.Equal(3344656, result.PurchaseOrder.Id);
        Assert.Equal(48.50m, result.PurchaseOrder.Total);
        Assert.Equal(PurchaseOrderStatus.Processed, result.PurchaseOrder.Status);
        Assert.Contains(result.AppliedRules, r => r.Rule.StartsWith("BR1"));
        Assert.Contains(result.AppliedRules, r => r.Rule.StartsWith("BR2") && r.Reference is not null);

        var customer = await scope.ServiceProvider.GetRequiredService<ICustomerRepository>().GetByIdAsync(4567890);
        Assert.Equal(MembershipType.BookClub, customer!.Membership);
    }

    [Fact]
    public async Task Unknown_product_is_a_validation_error()
    {
        using var provider = BuildProvider();
        using var scope = provider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IPurchaseOrderService>();

        var request = new CreatePurchaseOrderRequest
        {
            CustomerId = 4567890,
            Items = new() { new PurchaseOrderLineRequest { ProductId = 999 } }
        };

        await Assert.ThrowsAsync<RequestValidationException>(() => service.CreateAsync(request));
    }

    [Fact]
    public async Task Unknown_customer_is_not_found()
    {
        using var provider = BuildProvider();
        using var scope = provider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IPurchaseOrderService>();

        var request = ExampleRequest();
        var unknownCustomer = new CreatePurchaseOrderRequest { CustomerId = 1, Items = request.Items };

        await Assert.ThrowsAsync<NotFoundException>(() => service.CreateAsync(unknownCustomer));
    }
}
