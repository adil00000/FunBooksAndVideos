using FunBooksAndVideos.Application.Contracts.Responses;

namespace FunBooksAndVideos.Application.Services;

public interface IProductService
{
    Task<IReadOnlyList<ProductResponse>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<ProductResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}
