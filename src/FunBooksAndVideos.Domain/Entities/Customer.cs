using FunBooksAndVideos.Domain.Enums;
using FunBooksAndVideos.Domain.Exceptions;

namespace FunBooksAndVideos.Domain.Entities;

public sealed class Customer
{
    public Customer(int id, string name, string email, Address shippingAddress,
        MembershipType membership = MembershipType.None)
    {
        if (id <= 0)
        {
            throw new DomainException("Customer id must be a positive number.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Customer name is required.");
        }

        Id = id;
        Name = name;
        Email = email ?? string.Empty;
        ShippingAddress = shippingAddress ?? throw new DomainException("Shipping address is required.");
        Membership = membership;
    }

    public int Id { get; }

    public string Name { get; }

    public string Email { get; }

    public Address ShippingAddress { get; }

    public MembershipType Membership { get; private set; }

    public bool HasMembership(MembershipType membershipType) =>
        membershipType != MembershipType.None && (Membership & membershipType) == membershipType;

    /// <summary>
    /// Activates a membership immediately. Memberships combine, so a customer with
    /// the Book Club who buys the Video Club becomes Premium.
    /// </summary>
    public void ActivateMembership(MembershipType membershipType)
    {
        if (membershipType == MembershipType.None)
        {
            throw new DomainException("Cannot activate an empty membership.");
        }

        Membership |= membershipType;
    }
}
