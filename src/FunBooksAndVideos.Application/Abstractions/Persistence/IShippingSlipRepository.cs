using FunBooksAndVideos.Domain.Entities;

namespace FunBooksAndVideos.Application.Abstractions.Persistence;

public interface IShippingSlipRepository
{
    Task<ShippingSlip?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ShippingSlip>> GetByPurchaseOrderIdAsync(int purchaseOrderId, CancellationToken cancellationToken = default);

    Task AddAsync(ShippingSlip shippingSlip, CancellationToken cancellationToken = default);
}
