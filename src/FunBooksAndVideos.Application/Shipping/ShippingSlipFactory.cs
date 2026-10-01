using FunBooksAndVideos.Application.Abstractions.Shipping;
using FunBooksAndVideos.Domain.Entities;
using FunBooksAndVideos.Domain.Exceptions;

namespace FunBooksAndVideos.Application.Shipping;

public sealed class ShippingSlipFactory : IShippingSlipFactory
{
    private readonly TimeProvider _timeProvider;

    public ShippingSlipFactory(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    public ShippingSlip Create(PurchaseOrder purchaseOrder, Customer customer)
    {
        ArgumentNullException.ThrowIfNull(purchaseOrder);
        ArgumentNullException.ThrowIfNull(customer);

        if (purchaseOrder.CustomerId != customer.Id)
        {
            throw new DomainException("The purchase order does not belong to this customer.");
        }

        var lines = purchaseOrder.PhysicalItems
            .Select(item => new ShippingSlipLine(item.Product.Id, $"{item.Product.Type} \"{item.Product.Name}\"", item.Quantity))
            .ToList();

        return new ShippingSlip(
            Guid.NewGuid(),
            purchaseOrder.Id,
            customer.Id,
            customer.Name,
            customer.ShippingAddress,
            lines,
            _timeProvider.GetUtcNow());
    }
}
