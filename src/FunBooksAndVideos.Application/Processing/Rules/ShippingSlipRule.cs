using FunBooksAndVideos.Application.Abstractions.Persistence;
using FunBooksAndVideos.Application.Abstractions.Shipping;

namespace FunBooksAndVideos.Application.Processing.Rules;

/// <summary>
/// BR2. If the purchase order contains a physical product, a shipping slip has to be generated.
/// </summary>
public sealed class ShippingSlipRule : IPurchaseOrderRule
{
    private readonly IShippingSlipFactory _shippingSlipFactory;
    private readonly IShippingSlipRepository _shippingSlipRepository;

    public ShippingSlipRule(IShippingSlipFactory shippingSlipFactory, IShippingSlipRepository shippingSlipRepository)
    {
        _shippingSlipFactory = shippingSlipFactory;
        _shippingSlipRepository = shippingSlipRepository;
    }

    public string Name => "BR2 - Generate shipping slip";

    public int Order => 20;

    public bool IsApplicable(PurchaseOrderContext context) => context.PurchaseOrder.ContainsPhysicalProducts;

    public async Task ApplyAsync(PurchaseOrderContext context, CancellationToken cancellationToken = default)
    {
        var slip = _shippingSlipFactory.Create(context.PurchaseOrder, context.Customer);

        await _shippingSlipRepository.AddAsync(slip, cancellationToken);

        context.AddOutcome(new RuleOutcome(
            Name,
            $"Generated shipping slip with {slip.Lines.Count} line(s) for purchase order {slip.PurchaseOrderId}.",
            slip.Id.ToString()));
    }
}
