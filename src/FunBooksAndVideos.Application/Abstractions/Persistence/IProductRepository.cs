using FunBooksAndVideos.Domain.Entities;

namespace FunBooksAndVideos.Application.Abstractions.Persistence;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default);
}
