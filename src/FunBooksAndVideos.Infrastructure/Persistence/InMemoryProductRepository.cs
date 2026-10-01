using System.Collections.Concurrent;
using FunBooksAndVideos.Application.Abstractions.Persistence;
using FunBooksAndVideos.Domain.Entities;

namespace FunBooksAndVideos.Infrastructure.Persistence;

public sealed class InMemoryProductRepository : IProductRepository
{
    private readonly ConcurrentDictionary<int, Product> _products;

    public InMemoryProductRepository(IEnumerable<Product> products)
    {
        _products = new ConcurrentDictionary<int, Product>(products.ToDictionary(p => p.Id));
    }

    public Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        Task.FromResult(_products.TryGetValue(id, out var product) ? product : null);

    public Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<Product>>(_products.Values.OrderBy(p => p.Id).ToList());
}
