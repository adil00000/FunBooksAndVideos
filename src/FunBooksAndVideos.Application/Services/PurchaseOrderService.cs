using FunBooksAndVideos.Application.Abstractions.Persistence;
using FunBooksAndVideos.Application.Contracts.Mapping;
using FunBooksAndVideos.Application.Contracts.Requests;
using FunBooksAndVideos.Application.Contracts.Responses;
using FunBooksAndVideos.Application.Exceptions;
using FunBooksAndVideos.Application.Processing;
using FunBooksAndVideos.Domain.Entities;

namespace FunBooksAndVideos.Application.Services;

/// <summary>
/// Turns an API request into a <see cref="PurchaseOrder"/>, stores it and hands it to the
/// <see cref="IPurchaseOrderProcessor"/>. Business rules live in the processor's rules, not here.
/// </summary>
public sealed class PurchaseOrderService : IPurchaseOrderService
{
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IPurchaseOrderProcessor _processor;
    private readonly TimeProvider _timeProvider;

    public PurchaseOrderService(
        IPurchaseOrderRepository purchaseOrderRepository,
        IProductRepository productRepository,
        ICustomerRepository customerRepository,
        IPurchaseOrderProcessor processor,
        TimeProvider timeProvider)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
        _processor = processor;
        _timeProvider = timeProvider;
    }

    public async Task<ProcessedPurchaseOrderResponse> CreateAsync(CreatePurchaseOrderRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.Items is null || request.Items.Count == 0)
        {
            throw new RequestValidationException(nameof(request.Items), "A purchase order must contain at least one item line.");
        }

        _ = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken)
            ?? throw new NotFoundException("Customer", request.CustomerId);

        var items = await BuildItemsAsync(request.Items, cancellationToken);

        var id = await _purchaseOrderRepository.NextIdAsync(cancellationToken);
        var purchaseOrder = new PurchaseOrder(id, request.CustomerId, items, _timeProvider.GetUtcNow());

        await _purchaseOrderRepository.AddAsync(purchaseOrder, cancellationToken);

        var result = await _processor.ProcessAsync(purchaseOrder, cancellationToken);

        return result.ToResponse();
    }

    public async Task<PurchaseOrderResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        (await _purchaseOrderRepository.GetByIdAsync(id, cancellationToken))?.ToResponse();

    public async Task<IReadOnlyList<PurchaseOrderResponse>> GetAllAsync(CancellationToken cancellationToken = default) =>
        (await _purchaseOrderRepository.GetAllAsync(cancellationToken)).Select(o => o.ToResponse()).ToList();

    private async Task<List<PurchaseOrderItem>> BuildItemsAsync(
        IEnumerable<PurchaseOrderLineRequest> lines, CancellationToken cancellationToken)
    {
        var items = new List<PurchaseOrderItem>();
        var unknownProductIds = new List<int>();

        foreach (var line in lines)
        {
            var product = await _productRepository.GetByIdAsync(line.ProductId, cancellationToken);
            if (product is null)
            {
                unknownProductIds.Add(line.ProductId);
                continue;
            }

            items.Add(new PurchaseOrderItem(product, line.Quantity));
        }

        if (unknownProductIds.Count > 0)
        {
            throw new RequestValidationException(
                "Items",
                $"Unknown product id(s): {string.Join(", ", unknownProductIds)}.");
        }

        return items;
    }
}
