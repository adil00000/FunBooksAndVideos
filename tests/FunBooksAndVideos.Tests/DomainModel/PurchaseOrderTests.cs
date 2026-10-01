using FunBooksAndVideos.Domain.Entities;
using FunBooksAndVideos.Domain.Enums;
using FunBooksAndVideos.Domain.Exceptions;

namespace FunBooksAndVideos.Tests.DomainModel;

public class PurchaseOrderTests
{
    [Fact]
    public void Example_order_from_brief_totals_48_50()
    {
        var order = TestData.ExampleOrder();

        Assert.Equal(48.50m, order.Total);
        Assert.Equal(3, order.Items.Count);
        Assert.True(order.ContainsMemberships);
        Assert.True(order.ContainsPhysicalProducts);
    }

    [Fact]
    public void Total_includes_quantities()
    {
        var order = new PurchaseOrder(1, TestData.CustomerId,
            new[] { new PurchaseOrderItem(TestData.GirlOnTheTrain(), 3) }, DateTimeOffset.UtcNow);

        Assert.Equal(31.50m, order.Total);
    }

    [Fact]
    public void Order_without_items_is_rejected()
    {
        Assert.Throws<DomainException>(() =>
            new PurchaseOrder(1, TestData.CustomerId, Array.Empty<PurchaseOrderItem>(), DateTimeOffset.UtcNow));
    }

    [Fact]
    public void Membership_quantity_must_be_one()
    {
        Assert.Throws<DomainException>(() => new PurchaseOrderItem(TestData.BookClub(), 2));
    }

    [Fact]
    public void Video_only_order_has_no_physical_products()
    {
        var order = TestData.Order(TestData.FirstAidVideo());

        Assert.False(order.ContainsPhysicalProducts);
    }

    [Fact]
    public void Order_cannot_be_processed_twice()
    {
        var order = TestData.ExampleOrder();
        order.MarkAsProcessed(DateTimeOffset.UtcNow);

        Assert.Equal(PurchaseOrderStatus.Processed, order.Status);
        Assert.Throws<DomainException>(() => order.MarkAsProcessed(DateTimeOffset.UtcNow));
    }
}
