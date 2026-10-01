using FunBooksAndVideos.Application.Contracts.Responses;
using FunBooksAndVideos.Application.Processing;
using FunBooksAndVideos.Domain.Entities;

namespace FunBooksAndVideos.Application.Contracts.Mapping;

/// <summary>Maps domain objects to API contracts so the domain is never exposed directly.</summary>
public static class ResponseMapper
{
    public static AddressResponse ToResponse(this Address address) =>
        new(address.Line1, address.City, address.PostCode, address.Country);

    public static ProductResponse ToResponse(this Product product) =>
        new(product.Id,
            product.Name,
            product.Type,
            product.Price,
            product.IsPhysical,
            (product as MembershipProduct)?.MembershipType);

    public static CustomerResponse ToResponse(this Customer customer) =>
        new(customer.Id, customer.Name, customer.Email, customer.Membership, customer.ShippingAddress.ToResponse());

    public static PurchaseOrderResponse ToResponse(this PurchaseOrder order) =>
        new(order.Id,
            order.CustomerId,
            order.Total,
            order.Status,
            order.CreatedAt,
            order.ProcessedAt,
            order.Items
                .Select(i => new PurchaseOrderLineResponse(
                    i.Product.Id, i.Product.Name, i.Product.Type, i.Quantity, i.UnitPrice, i.LineTotal))
                .ToList());

    public static ProcessedPurchaseOrderResponse ToResponse(this ProcessingResult result) =>
        new(result.PurchaseOrder.ToResponse(),
            result.Outcomes.Select(o => new RuleOutcomeResponse(o.Rule, o.Description, o.Reference)).ToList());

    public static ShippingSlipResponse ToResponse(this ShippingSlip slip) =>
        new(slip.Id,
            slip.PurchaseOrderId,
            slip.CustomerId,
            slip.RecipientName,
            slip.ShippingAddress.ToResponse(),
            slip.CreatedAt,
            slip.Lines.Select(l => new ShippingSlipLineResponse(l.ProductId, l.Description, l.Quantity)).ToList());
}
