using FunBooksAndVideos.Domain.Enums;
using FunBooksAndVideos.Domain.Exceptions;

namespace FunBooksAndVideos.Tests.DomainModel;

public class CustomerTests
{
    [Fact]
    public void ActivateMembership_sets_membership()
    {
        var customer = TestData.Customer();

        customer.ActivateMembership(MembershipType.BookClub);

        Assert.Equal(MembershipType.BookClub, customer.Membership);
        Assert.True(customer.HasMembership(MembershipType.BookClub));
        Assert.False(customer.HasMembership(MembershipType.VideoClub));
    }

    [Fact]
    public void Book_and_video_club_together_make_premium()
    {
        var customer = TestData.Customer(MembershipType.BookClub);

        customer.ActivateMembership(MembershipType.VideoClub);

        Assert.Equal(MembershipType.Premium, customer.Membership);
    }

    [Fact]
    public void ActivateMembership_rejects_none()
    {
        var customer = TestData.Customer();

        Assert.Throws<DomainException>(() => customer.ActivateMembership(MembershipType.None));
    }
}
