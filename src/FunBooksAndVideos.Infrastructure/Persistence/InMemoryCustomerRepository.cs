using System.Collections.Concurrent;
using FunBooksAndVideos.Application.Abstractions.Persistence;
using FunBooksAndVideos.Domain.Entities;

namespace FunBooksAndVideos.Infrastructure.Persistence;

public sealed class InMemoryCustomerRepository : ICustomerRepository
{
    private readonly ConcurrentDictionary<int, Customer> _customers;

    public InMemoryCustomerRepository(IEnumerable<Customer> customers)
    {
        _customers = new ConcurrentDictionary<int, Customer>(customers.ToDictionary(c => c.Id));
    }

    public Task<Customer?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        Task.FromResult(_customers.TryGetValue(id, out var customer) ? customer : null);

    public Task<IReadOnlyList<Customer>> GetAllAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<Customer>>(_customers.Values.OrderBy(c => c.Id).ToList());

    public Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        _customers[customer.Id] = customer;
        return Task.CompletedTask;
    }
}
