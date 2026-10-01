using FunBooksAndVideos.Domain.Entities;
using FunBooksAndVideos.Domain.Enums;

namespace FunBooksAndVideos.Tests;

internal static class TestData
{
    public const int CustomerId = 4567890;

    public static Video FirstAidVideo() => new(1, "Comprehensive First Aid Training", 15.00m, TimeSpan.FromMinutes(95));

    public static Book GirlOnTheTrain() => new(2, "The Girl on the Train", 10.50m, "Paula Hawkins");

    public static MembershipProduct BookClub() => new(3, "Book Club Membership", 23.00m, MembershipType.BookClub);

    public static MembershipProduct VideoClub() => new(4, "Video Club Membership", 25.00m, MembershipType.VideoClub);

    public static Customer Customer(MembershipType membership = MembershipType.None) =>
        new(CustomerId, "Jane Doe", "jane@example.com",
            new Address("1 High Street", "London", "SW1A 1AA", "United Kingdom"), membership);

    public static PurchaseOrder Order(params Product[] products) =>
        new(3344656, CustomerId, products.Select(p => new PurchaseOrderItem(p)), DateTimeOffset.UtcNow);

    /// <summary>The purchase order from the brief.</summary>
    public static PurchaseOrder ExampleOrder() => Order(FirstAidVideo(), GirlOnTheTrain(), BookClub());
}
