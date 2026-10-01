using FunBooksAndVideos.Domain.Enums;
using FunBooksAndVideos.Domain.Exceptions;

namespace FunBooksAndVideos.Domain.Entities;

/// <summary>
/// A purchase order: PO id, customer id, item lines and a total price.
/// </summary>
public sealed class PurchaseOrder
{
    private readonly List<PurchaseOrderItem> _items;

    public PurchaseOrder(int id, int customerId, IEnumerable<PurchaseOrderItem> items, DateTimeOffset createdAt)
    {
        if (id <= 0)
        {
            throw new DomainException("Purchase order id must be a positive number.");
        }

        if (customerId <= 0)
        {
            throw new DomainException("Customer id must be a positive number.");
        }

        _items = items?.ToList() ?? throw new DomainException("Item lines are required.");

        if (_items.Count == 0)
        {
            throw new DomainException("A purchase order must contain at least one item line.");
        }

        var duplicateMembership = _items
            .Select(i => i.Product)
            .OfType<MembershipProduct>()
            .GroupBy(m => m.Id)
            .FirstOrDefault(g => g.Count() > 1);

        if (duplicateMembership is not null)
        {
            throw new DomainException($"Membership '{duplicateMembership.First().Name}' appears more than once.");
        }

        Id = id;
        CustomerId = customerId;
        CreatedAt = createdAt;
        Status = PurchaseOrderStatus.Created;
    }

    public int Id { get; }

    public int CustomerId { get; }

    public IReadOnlyList<PurchaseOrderItem> Items => _items.AsReadOnly();

    public decimal Total => _items.Sum(i => i.LineTotal);

    public PurchaseOrderStatus Status { get; private set; }

    public DateTimeOffset CreatedAt { get; }

    public DateTimeOffset? ProcessedAt { get; private set; }

    public IEnumerable<PurchaseOrderItem> PhysicalItems => _items.Where(i => i.Product.IsPhysical);

    public IEnumerable<MembershipProduct> Memberships => _items.Select(i => i.Product).OfType<MembershipProduct>();

    public bool ContainsPhysicalProducts => PhysicalItems.Any();

    public bool ContainsMemberships => Memberships.Any();

    public void MarkAsProcessed(DateTimeOffset processedAt)
    {
        if (Status == PurchaseOrderStatus.Processed)
        {
            throw new DomainException($"Purchase order {Id} has already been processed.");
        }

        Status = PurchaseOrderStatus.Processed;
        ProcessedAt = processedAt;
    }
}
