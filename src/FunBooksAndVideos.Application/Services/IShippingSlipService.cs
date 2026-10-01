using FunBooksAndVideos.Application.Contracts.Responses;

namespace FunBooksAndVideos.Application.Services;

public interface IShippingSlipService
{
    Task<ShippingSlipResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ShippingSlipResponse>> GetByPurchaseOrderIdAsync(int purchaseOrderId, CancellationToken cancellationToken = default);
}
