using FunBooksAndVideos.Domain.Entities;

namespace FunBooksAndVideos.Application.Processing;

public interface IPurchaseOrderProcessor
{
    Task<ProcessingResult> ProcessAsync(PurchaseOrder purchaseOrder, CancellationToken cancellationToken = default);
}
