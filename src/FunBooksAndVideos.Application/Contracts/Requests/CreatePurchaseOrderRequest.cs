using System.ComponentModel.DataAnnotations;

namespace FunBooksAndVideos.Application.Contracts.Requests;

/// <summary>Request to create and process a purchase order.</summary>
public sealed class CreatePurchaseOrderRequest
{
    /// <example>4567890</example>
    [Range(1, int.MaxValue)]
    public int CustomerId { get; init; }

    [Required]
    [MinLength(1)]
    public List<PurchaseOrderLineRequest> Items { get; init; } = new();
}

/// <summary>One item line: a product id (book, video or membership) and a quantity.</summary>
public sealed class PurchaseOrderLineRequest
{
    /// <example>2</example>
    [Range(1, int.MaxValue)]
    public int ProductId { get; init; }

    /// <example>1</example>
    [Range(1, 1000)]
    public int Quantity { get; init; } = 1;
}
