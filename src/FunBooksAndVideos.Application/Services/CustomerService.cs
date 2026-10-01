using FunBooksAndVideos.Application.Abstractions.Persistence;
using FunBooksAndVideos.Application.Contracts.Mapping;
using FunBooksAndVideos.Application.Contracts.Responses;

namespace FunBooksAndVideos.Application.Services;

public sealed class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<IReadOnlyList<CustomerResponse>> GetAllAsync(CancellationToken cancellationToken = default) =>
        (await _customerRepository.GetAllAsync(cancellationToken)).Select(c => c.ToResponse()).ToList();

    public async Task<CustomerResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        (await _customerRepository.GetByIdAsync(id, cancellationToken))?.ToResponse();
}
