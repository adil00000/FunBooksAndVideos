using FunBooksAndVideos.Application.Abstractions.Persistence;
using FunBooksAndVideos.Application.Contracts.Mapping;
using FunBooksAndVideos.Application.Contracts.Responses;

namespace FunBooksAndVideos.Application.Services;

public sealed class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IReadOnlyList<ProductResponse>> GetAllAsync(CancellationToken cancellationToken = default) =>
        (await _productRepository.GetAllAsync(cancellationToken)).Select(p => p.ToResponse()).ToList();

    public async Task<ProductResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        (await _productRepository.GetByIdAsync(id, cancellationToken))?.ToResponse();
}
