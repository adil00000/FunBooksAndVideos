using FunBooksAndVideos.Application.Contracts.Requests;
using FunBooksAndVideos.Application.Contracts.Responses;

namespace FunBooksAndVideos.Application.Services;

public interface IPurchaseOrderService
{
    /// <summary>Creates a purchase order and runs it through the purchase order processor.</summary>
    Task<ProcessedPurchaseOrderResponse> CreateAsync(CreatePurchaseOrderRequest request, CancellationToken cancellationToken = default);

    Task<PurchaseOrderResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PurchaseOrderResponse>> GetAllAsync(CancellationToken cancellationToken = default);
}
