using FunBooksAndVideos.Application.Abstractions.Persistence;

namespace FunBooksAndVideos.Application.Processing.Rules;

/// <summary>
/// BR1. If the purchase order contains a membership, it has to be activated
/// in the customer account immediately.
/// </summary>
public sealed class MembershipActivationRule : IPurchaseOrderRule
{
    private readonly ICustomerRepository _customerRepository;

    public MembershipActivationRule(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public string Name => "BR1 - Activate membership";

    public int Order => 10;

    public bool IsApplicable(PurchaseOrderContext context) => context.PurchaseOrder.ContainsMemberships;

    public async Task ApplyAsync(PurchaseOrderContext context, CancellationToken cancellationToken = default)
    {
        var customer = context.Customer;

        foreach (var membership in context.PurchaseOrder.Memberships)
        {
            customer.ActivateMembership(membership.MembershipType);

            context.AddOutcome(new RuleOutcome(
                Name,
                $"Activated {membership.MembershipType} membership for customer {customer.Id}. Current membership: {customer.Membership}."));
        }

        await _customerRepository.UpdateAsync(customer, cancellationToken);
    }
}
