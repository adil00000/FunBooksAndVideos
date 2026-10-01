namespace FunBooksAndVideos.Application.Processing.Rules;

/// <summary>
/// A single business rule applied when a purchase order is processed (Strategy pattern).
/// To add a rule: implement this interface and register it in DependencyInjection.
/// The processor never changes (Open/Closed).
/// </summary>
public interface IPurchaseOrderRule
{
    /// <summary>Display name, e.g. "BR1 - Activate membership".</summary>
    string Name { get; }

    /// <summary>Rules run in ascending order.</summary>
    int Order { get; }

    bool IsApplicable(PurchaseOrderContext context);

    Task ApplyAsync(PurchaseOrderContext context, CancellationToken cancellationToken = default);
}
