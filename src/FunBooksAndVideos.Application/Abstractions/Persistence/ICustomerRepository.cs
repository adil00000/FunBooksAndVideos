using FunBooksAndVideos.Domain.Entities;

namespace FunBooksAndVideos.Application.Abstractions.Persistence;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Customer>> GetAllAsync(CancellationToken cancellationToken = default);

    Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default);
}
