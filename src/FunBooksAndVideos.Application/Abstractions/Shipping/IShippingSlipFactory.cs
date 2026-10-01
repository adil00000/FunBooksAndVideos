using FunBooksAndVideos.Domain.Entities;

namespace FunBooksAndVideos.Application.Abstractions.Shipping;

/// <summary>Builds a shipping slip for the physical items of an order.</summary>
public interface IShippingSlipFactory
{
    ShippingSlip Create(PurchaseOrder purchaseOrder, Customer customer);
}
