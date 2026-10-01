using FunBooksAndVideos.Domain.Exceptions;

namespace FunBooksAndVideos.Domain.Entities;

/// <summary>One item line on a purchase order (a product or a membership).</summary>
public sealed class PurchaseOrderItem
{
    public PurchaseOrderItem(Product product, int quantity = 1)
    {
        Product = product ?? throw new DomainException("An item line needs a product.");

        if (quantity <= 0)
        {
            throw new DomainException($"Quantity for '{product.Name}' must be at least 1.");
        }

        if (product is MembershipProduct && quantity != 1)
        {
            throw new DomainException($"Membership '{product.Name}' can only be ordered once per purchase order.");
        }

        Quantity = quantity;
        UnitPrice = product.Price; // price is captured at the time of ordering
    }

    public Product Product { get; }

    public int Quantity { get; }

    public decimal UnitPrice { get; }

    public decimal LineTotal => UnitPrice * Quantity;
}
