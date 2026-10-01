using FunBooksAndVideos.Domain.Entities;

namespace FunBooksAndVideos.Application.Abstractions.Persistence;

public interface IPurchaseOrderRepository
{
    Task<int> NextIdAsync(CancellationToken cancellationToken = default);

    Task<PurchaseOrder?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PurchaseOrder>> GetAllAsync(CancellationToken cancellationToken = default);

    Task AddAsync(PurchaseOrder purchaseOrder, CancellationToken cancellationToken = default);

    Task UpdateAsync(PurchaseOrder purchaseOrder, CancellationToken cancellationToken = default);
}
