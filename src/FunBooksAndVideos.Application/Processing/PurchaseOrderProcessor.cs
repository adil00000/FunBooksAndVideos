using FunBooksAndVideos.Application.Abstractions.Persistence;
using FunBooksAndVideos.Application.Exceptions;
using FunBooksAndVideos.Application.Processing.Rules;
using FunBooksAndVideos.Domain.Entities;
using FunBooksAndVideos.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace FunBooksAndVideos.Application.Processing;

/// <summary>
/// Runs every registered <see cref="IPurchaseOrderRule"/> that applies to an order.
/// The processor knows nothing about specific rules: they are injected by the IoC container.
/// </summary>
public sealed class PurchaseOrderProcessor : IPurchaseOrderProcessor
{
    private readonly IReadOnlyList<IPurchaseOrderRule> _rules;
    private readonly ICustomerRepository _customerRepository;
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;
    private readonly TimeProvider _timeProvider;
    private readonly ILogger<PurchaseOrderProcessor> _logger;

    public PurchaseOrderProcessor(
        IEnumerable<IPurchaseOrderRule> rules,
        ICustomerRepository customerRepository,
        IPurchaseOrderRepository purchaseOrderRepository,
        TimeProvider timeProvider,
        ILogger<PurchaseOrderProcessor> logger)
    {
        _rules = rules.OrderBy(r => r.Order).ToList();
        _customerRepository = customerRepository;
        _purchaseOrderRepository = purchaseOrderRepository;
        _timeProvider = timeProvider;
        _logger = logger;
    }

    public async Task<ProcessingResult> ProcessAsync(PurchaseOrder purchaseOrder, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(purchaseOrder);

        if (purchaseOrder.Status == PurchaseOrderStatus.Processed)
        {
            throw new ConflictException($"Purchase order {purchaseOrder.Id} has already been processed.");
        }

        var customer = await _customerRepository.GetByIdAsync(purchaseOrder.CustomerId, cancellationToken)
                       ?? throw new NotFoundException("Customer", purchaseOrder.CustomerId);

        var context = new PurchaseOrderContext(purchaseOrder, customer);

        foreach (var rule in _rules)
        {
            if (!rule.IsApplicable(context))
            {
                continue;
            }

            _logger.LogInformation("Applying rule {Rule} to purchase order {PurchaseOrderId}", rule.Name, purchaseOrder.Id);
            await rule.ApplyAsync(context, cancellationToken);
        }

        purchaseOrder.MarkAsProcessed(_timeProvider.GetUtcNow());
        await _purchaseOrderRepository.UpdateAsync(purchaseOrder, cancellationToken);

        return new ProcessingResult(purchaseOrder, context.Outcomes);
    }
}
