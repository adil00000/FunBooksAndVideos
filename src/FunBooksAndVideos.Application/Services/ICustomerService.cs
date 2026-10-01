using FunBooksAndVideos.Application.Contracts.Responses;

namespace FunBooksAndVideos.Application.Services;

public interface ICustomerService
{
    Task<IReadOnlyList<CustomerResponse>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<CustomerResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}
