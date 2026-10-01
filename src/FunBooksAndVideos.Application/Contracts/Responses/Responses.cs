using FunBooksAndVideos.Domain.Enums;

namespace FunBooksAndVideos.Application.Contracts.Responses;

public sealed record AddressResponse(string Line1, string City, string PostCode, string Country);

public sealed record ProductResponse(
    int Id,
    string Name,
    ProductType Type,
    decimal Price,
    bool IsPhysical,
    MembershipType? MembershipType);

public sealed record CustomerResponse(
    int Id,
    string Name,
    string Email,
    MembershipType Membership,
    AddressResponse ShippingAddress);

public sealed record PurchaseOrderLineResponse(
    int ProductId,
    string ProductName,
    ProductType ProductType,
    int Quantity,
    decimal UnitPrice,
    decimal LineTotal);

public sealed record PurchaseOrderResponse(
    int Id,
    int CustomerId,
    decimal Total,
    PurchaseOrderStatus Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset? ProcessedAt,
    IReadOnlyList<PurchaseOrderLineResponse> Items);

public sealed record RuleOutcomeResponse(string Rule, string Description, string? Reference);

public sealed record ProcessedPurchaseOrderResponse(
    PurchaseOrderResponse PurchaseOrder,
    IReadOnlyList<RuleOutcomeResponse> AppliedRules);

public sealed record ShippingSlipLineResponse(int ProductId, string Description, int Quantity);

public sealed record ShippingSlipResponse(
    Guid Id,
    int PurchaseOrderId,
    int CustomerId,
    string RecipientName,
    AddressResponse ShippingAddress,
    DateTimeOffset CreatedAt,
    IReadOnlyList<ShippingSlipLineResponse> Lines);
