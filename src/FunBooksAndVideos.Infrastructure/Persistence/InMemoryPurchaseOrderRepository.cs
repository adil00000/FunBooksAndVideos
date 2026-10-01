using System.Collections.Concurrent;
using FunBooksAndVideos.Application.Abstractions.Persistence;
using FunBooksAndVideos.Domain.Entities;

namespace FunBooksAndVideos.Infrastructure.Persistence;

public sealed class InMemoryPurchaseOrderRepository : IPurchaseOrderRepository
{
    private readonly ConcurrentDictionary<int, PurchaseOrder> _orders = new();
    private int _lastId;

    public InMemoryPurchaseOrderRepository(int firstId = SeedData.FirstPurchaseOrderId)
    {
        _lastId = firstId - 1;
    }

    public Task<int> NextIdAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(Interlocked.Increment(ref _lastId));

    public Task<PurchaseOrder?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        Task.FromResult(_orders.TryGetValue(id, out var order) ? order : null);

    public Task<IReadOnlyList<PurchaseOrder>> GetAllAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<PurchaseOrder>>(_orders.Values.OrderBy(o => o.Id).ToList());

    public Task AddAsync(PurchaseOrder purchaseOrder, CancellationToken cancellationToken = default)
    {
        if (!_orders.TryAdd(purchaseOrder.Id, purchaseOrder))
        {
            throw new InvalidOperationException($"Purchase order {purchaseOrder.Id} already exists.");
        }

        return Task.CompletedTask;
    }

    public Task UpdateAsync(PurchaseOrder purchaseOrder, CancellationToken cancellationToken = default)
    {
        _orders[purchaseOrder.Id] = purchaseOrder;
        return Task.CompletedTask;
    }
}
