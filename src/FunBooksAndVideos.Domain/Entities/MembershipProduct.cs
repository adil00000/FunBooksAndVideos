using FunBooksAndVideos.Domain.Enums;
using FunBooksAndVideos.Domain.Exceptions;

namespace FunBooksAndVideos.Domain.Entities;

/// <summary>A purchasable membership request (Book Club, Video Club or Premium).</summary>
public sealed class MembershipProduct : Product
{
    public MembershipProduct(int id, string name, decimal price, MembershipType membershipType)
        : base(id, name, price)
    {
        if (membershipType == MembershipType.None)
        {
            throw new DomainException("A membership product must grant a membership.");
        }

        MembershipType = membershipType;
    }

    public MembershipType MembershipType { get; }

    public override ProductType Type => ProductType.Membership;

    public override bool IsPhysical => false;
}
