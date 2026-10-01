using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using FunBooksAndVideos.Application.Contracts.Responses;
using FunBooksAndVideos.Domain.Enums;
using Microsoft.AspNetCore.Mvc.Testing;

namespace FunBooksAndVideos.Tests.Integration;

public class PurchaseOrdersApiTests
{
    private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    private static readonly object ExampleOrder = new
    {
        customerId = 4567890,
        items = new[]
        {
            new { productId = 1, quantity = 1 },
            new { productId = 2, quantity = 1 },
            new { productId = 3, quantity = 1 }
        }
    };

    [Fact]
    public async Task Post_example_order_returns_201_and_applies_rules()
    {
        await using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/purchase-orders", ExampleOrder);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Headers.Location);

        var body = await response.Content.ReadFromJsonAsync<ProcessedPurchaseOrderResponse>(Json);
        Assert.NotNull(body);
        Assert.Equal(48.50m, body!.PurchaseOrder.Total);
        Assert.Equal(2, body.AppliedRules.Count);

        var customer = await client.GetFromJsonAsync<CustomerResponse>("/api/customers/4567890", Json);
        Assert.Equal(MembershipType.BookClub, customer!.Membership);

        var slips = await client.GetFromJsonAsync<List<ShippingSlipResponse>>(
            $"/api/purchase-orders/{body.PurchaseOrder.Id}/shipping-slips", Json);
        Assert.Single(slips!);
    }

    [Fact]
    public async Task Post_with_unknown_product_returns_400()
    {
        await using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/purchase-orders",
            new { customerId = 4567890, items = new[] { new { productId = 999, quantity = 1 } } });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_for_unknown_customer_returns_404()
    {
        await using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/purchase-orders",
            new { customerId = 42, items = new[] { new { productId = 1, quantity = 1 } } });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Swagger_document_is_served()
    {
        await using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/swagger/v1/swagger.json");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
