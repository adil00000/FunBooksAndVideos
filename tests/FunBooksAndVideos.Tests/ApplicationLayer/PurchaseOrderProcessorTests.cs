using FunBooksAndVideos.Application.Exceptions;
using FunBooksAndVideos.Application.Processing;
using FunBooksAndVideos.Application.Processing.Rules;
using FunBooksAndVideos.Domain.Entities;
using FunBooksAndVideos.Domain.Enums;
using FunBooksAndVideos.Infrastructure.Persistence;
using Microsoft.Extensions.Logging.Abstractions;

namespace FunBooksAndVideos.Tests.ApplicationLayer;

public class PurchaseOrderProcessorTests
{
    private sealed class SpyRule : IPurchaseOrderRule
    {
        private readonly bool _applicable;
        private readonly List<string> _log;

        public SpyRule(string name, int order, bool applicable, List<string> log)
        {
            Name = name;
            Order = order;
            _applicable = applicable;
            _log = log;
        }

        public string Name { get; }

        public int Order { get; }

        public bool IsApplicable(PurchaseOrderContext context) => _applicable;

        public Task ApplyAsync(PurchaseOrderContext context, CancellationToken cancellationToken = default)
        {
            _log.Add(Name);
            context.AddOutcome(new RuleOutcome(Name, "ran"));
            return Task.CompletedTask;
        }
    }

    private static PurchaseOrderProcessor CreateProcessor(params IPurchaseOrderRule[] rules) =>
        new(rules,
            new InMemoryCustomerRepository(new[] { TestData.Customer() }),
            new InMemoryPurchaseOrderRepository(),
            TimeProvider.System,
            NullLogger<PurchaseOrderProcessor>.Instance);

    [Fact]
    public async Task Runs_only_applicable_rules_in_order()
    {
        var log = new List<string>();
        var processor = CreateProcessor(
            new SpyRule("second", 2, true, log),
            new SpyRule("skipped", 1, false, log),
            new SpyRule("first", 0, true, log));

        var result = await processor.ProcessAsync(TestData.ExampleOrder());

        Assert.Equal(new[] { "first", "second" }, log.ToArray());
        Assert.Equal(2, result.Outcomes.Count);
        Assert.Equal(PurchaseOrderStatus.Processed, result.PurchaseOrder.Status);
        Assert.NotNull(result.PurchaseOrder.ProcessedAt);
    }

    [Fact]
    public async Task Rejects_already_processed_order()
    {
        var processor = CreateProcessor();
        var order = TestData.ExampleOrder();
        await processor.ProcessAsync(order);

        await Assert.ThrowsAsync<ConflictException>(() => processor.ProcessAsync(order));
    }

    [Fact]
    public async Task Unknown_customer_is_reported()
    {
        var processor = new PurchaseOrderProcessor(
            Array.Empty<IPurchaseOrderRule>(),
            new InMemoryCustomerRepository(Array.Empty<Customer>()),
            new InMemoryPurchaseOrderRepository(),
            TimeProvider.System,
            NullLogger<PurchaseOrderProcessor>.Instance);

        await Assert.ThrowsAsync<NotFoundException>(() => processor.ProcessAsync(TestData.ExampleOrder()));
    }
}
