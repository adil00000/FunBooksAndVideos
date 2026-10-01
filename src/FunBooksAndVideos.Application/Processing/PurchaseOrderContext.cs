using FunBooksAndVideos.Domain.Entities;

namespace FunBooksAndVideos.Application.Processing;

/// <summary>
/// State shared by the rules while one purchase order is processed.
/// Rules report what they did through <see cref="AddOutcome"/>, so new rules
/// never require changes to this class or to the processor.
/// </summary>
public sealed class PurchaseOrderContext
{
    private readonly List<RuleOutcome> _outcomes = new();

    public PurchaseOrderContext(PurchaseOrder purchaseOrder, Customer customer)
    {
        PurchaseOrder = purchaseOrder ?? throw new ArgumentNullException(nameof(purchaseOrder));
        Customer = customer ?? throw new ArgumentNullException(nameof(customer));
    }

    public PurchaseOrder PurchaseOrder { get; }

    public Customer Customer { get; }

    public IReadOnlyList<RuleOutcome> Outcomes => _outcomes.AsReadOnly();

    public void AddOutcome(RuleOutcome outcome)
    {
        ArgumentNullException.ThrowIfNull(outcome);
        _outcomes.Add(outcome);
    }
}
