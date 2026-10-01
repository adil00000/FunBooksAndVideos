using FunBooksAndVideos.Application.Abstractions.Persistence;
using FunBooksAndVideos.Application.Contracts.Mapping;
using FunBooksAndVideos.Application.Contracts.Responses;

namespace FunBooksAndVideos.Application.Services;

public sealed class ShippingSlipService : IShippingSlipService
{
    private readonly IShippingSlipRepository _shippingSlipRepository;

    public ShippingSlipService(IShippingSlipRepository shippingSlipRepository)
    {
        _shippingSlipRepository = shippingSlipRepository;
    }

    public async Task<ShippingSlipResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        (await _shippingSlipRepository.GetByIdAsync(id, cancellationToken))?.ToResponse();

    public async Task<IReadOnlyList<ShippingSlipResponse>> GetByPurchaseOrderIdAsync(int purchaseOrderId, CancellationToken cancellationToken = default) =>
        (await _shippingSlipRepository.GetByPurchaseOrderIdAsync(purchaseOrderId, cancellationToken))
            .Select(s => s.ToResponse())
            .ToList();
}
