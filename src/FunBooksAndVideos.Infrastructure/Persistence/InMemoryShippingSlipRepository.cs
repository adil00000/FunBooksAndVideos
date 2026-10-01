using System.Collections.Concurrent;
using FunBooksAndVideos.Application.Abstractions.Persistence;
using FunBooksAndVideos.Domain.Entities;

namespace FunBooksAndVideos.Infrastructure.Persistence;

public sealed class InMemoryShippingSlipRepository : IShippingSlipRepository
{
    private readonly ConcurrentDictionary<Guid, ShippingSlip> _slips = new();

    public Task<ShippingSlip?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        Task.FromResult(_slips.TryGetValue(id, out var slip) ? slip : null);

    public Task<IReadOnlyList<ShippingSlip>> GetByPurchaseOrderIdAsync(int purchaseOrderId, CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<ShippingSlip>>(
            _slips.Values.Where(s => s.PurchaseOrderId == purchaseOrderId).OrderBy(s => s.CreatedAt).ToList());

    public Task AddAsync(ShippingSlip shippingSlip, CancellationToken cancellationToken = default)
    {
        _slips[shippingSlip.Id] = shippingSlip;
        return Task.CompletedTask;
    }
}
