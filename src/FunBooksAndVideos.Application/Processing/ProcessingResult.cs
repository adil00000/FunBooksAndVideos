using FunBooksAndVideos.Domain.Entities;

namespace FunBooksAndVideos.Application.Processing;

public sealed record ProcessingResult(PurchaseOrder PurchaseOrder, IReadOnlyList<RuleOutcome> Outcomes);
