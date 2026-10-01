using FunBooksAndVideos.Domain.Exceptions;

namespace FunBooksAndVideos.Domain.Entities;

public sealed record ShippingSlipLine(int ProductId, string Description, int Quantity);

/// <summary>Shipping slip generated for the physical products of a purchase order (BR2).</summary>
public sealed class ShippingSlip
{
    private readonly List<ShippingSlipLine> _lines;

    public ShippingSlip(Guid id, int purchaseOrderId, int customerId, string recipientName,
        Address shippingAddress, IEnumerable<ShippingSlipLine> lines, DateTimeOffset createdAt)
    {
        _lines = lines?.ToList() ?? new List<ShippingSlipLine>();

        if (_lines.Count == 0)
        {
            throw new DomainException("A shipping slip needs at least one line.");
        }

        Id = id;
        PurchaseOrderId = purchaseOrderId;
        CustomerId = customerId;
        RecipientName = recipientName;
        ShippingAddress = shippingAddress ?? throw new DomainException("Shipping address is required.");
        CreatedAt = createdAt;
    }

    public Guid Id { get; }

    public int PurchaseOrderId { get; }

    public int CustomerId { get; }

    public string RecipientName { get; }

    public Address ShippingAddress { get; }

    public IReadOnlyList<ShippingSlipLine> Lines => _lines.AsReadOnly();

    public DateTimeOffset CreatedAt { get; }
}
