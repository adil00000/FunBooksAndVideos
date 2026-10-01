using FunBooksAndVideos.Application.Processing;
using FunBooksAndVideos.Application.Processing.Rules;
using FunBooksAndVideos.Application.Shipping;
using FunBooksAndVideos.Domain.Enums;
using FunBooksAndVideos.Infrastructure.Persistence;

namespace FunBooksAndVideos.Tests.ApplicationLayer;

public class BusinessRuleTests
{
    [Fact]
    public async Task BR1_activates_membership_immediately()
    {
        var customer = TestData.Customer();
        var customers = new InMemoryCustomerRepository(new[] { customer });
        var rule = new MembershipActivationRule(customers);
        var context = new PurchaseOrderContext(TestData.ExampleOrder(), customer);

        Assert.True(rule.IsApplicable(context));
        await rule.ApplyAsync(context);

        var stored = await customers.GetByIdAsync(customer.Id);
        Assert.Equal(MembershipType.BookClub, stored!.Membership);
        Assert.Single(context.Outcomes);
    }

    [Fact]
    public void BR1_does_not_apply_without_membership()
    {
        var customer = TestData.Customer();
        var rule = new MembershipActivationRule(new InMemoryCustomerRepository(new[] { customer }));
        var context = new PurchaseOrderContext(TestData.Order(TestData.GirlOnTheTrain()), customer);

        Assert.False(rule.IsApplicable(context));
    }

    [Fact]
    public async Task BR2_generates_shipping_slip_for_physical_products_only()
    {
        var customer = TestData.Customer();
        var slips = new InMemoryShippingSlipRepository();
        var rule = new ShippingSlipRule(new ShippingSlipFactory(TimeProvider.System), slips);
        var order = TestData.ExampleOrder();
        var context = new PurchaseOrderContext(order, customer);

        Assert.True(rule.IsApplicable(context));
        await rule.ApplyAsync(context);

        var slip = Assert.Single(await slips.GetByPurchaseOrderIdAsync(order.Id));
        var line = Assert.Single(slip.Lines); // only the book; the video and membership are not shipped
        Assert.Equal(2, line.ProductId);
        Assert.Equal(customer.ShippingAddress, slip.ShippingAddress);
        Assert.Equal(slip.Id.ToString(), context.Outcomes.Single().Reference);
    }

    [Fact]
    public void BR2_does_not_apply_to_digital_only_orders()
    {
        var customer = TestData.Customer();
        var rule = new ShippingSlipRule(new ShippingSlipFactory(TimeProvider.System), new InMemoryShippingSlipRepository());
        var context = new PurchaseOrderContext(TestData.Order(TestData.FirstAidVideo(), TestData.VideoClub()), customer);

        Assert.False(rule.IsApplicable(context));
    }
}
