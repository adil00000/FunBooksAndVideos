namespace FunBooksAndVideos.Application.Processing;

/// <summary>What a business rule did while an order was processed.</summary>
/// <param name="Rule">Name of the rule that ran.</param>
/// <param name="Description">Human-readable description of the action.</param>
/// <param name="Reference">Optional id of anything the rule created (e.g. a shipping slip id).</param>
public sealed record RuleOutcome(string Rule, string Description, string? Reference = null);
