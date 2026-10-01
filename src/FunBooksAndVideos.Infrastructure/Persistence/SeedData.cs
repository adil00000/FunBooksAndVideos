using FunBooksAndVideos.Domain.Entities;
using FunBooksAndVideos.Domain.Enums;

namespace FunBooksAndVideos.Infrastructure.Persistence;

/// <summary>
/// Sample catalogue and customers. Products 1, 2 and 3 reproduce the purchase order
/// in the brief (total 48.50 for customer 4567890).
/// </summary>
public static class SeedData
{
    public const int FirstPurchaseOrderId = 3344656;

    public static IEnumerable<Product> Products() => new Product[]
    {
        new Video(1, "Comprehensive First Aid Training", 15.00m, TimeSpan.FromMinutes(95)),
        new Book(2, "The Girl on the Train", 10.50m, "Paula Hawkins"),
        new MembershipProduct(3, "Book Club Membership", 23.00m, MembershipType.BookClub),
        new MembershipProduct(4, "Video Club Membership", 25.00m, MembershipType.VideoClub),
        new MembershipProduct(5, "Premium Membership", 40.00m, MembershipType.Premium),
        new Book(6, "Clean Code", 29.99m, "Robert C. Martin"),
        new Video(7, "Introduction to C#", 12.00m, TimeSpan.FromMinutes(60))
    };

    public static IEnumerable<Customer> Customers() => new[]
    {
        new Customer(4567890, "Jane Doe", "jane.doe@example.com",
            new Address("1 High Street", "London", "SW1A 1AA", "United Kingdom")),
        new Customer(1234567, "John Smith", "john.smith@example.com",
            new Address("22 Market Road", "Manchester", "M1 1AE", "United Kingdom"),
            MembershipType.VideoClub)
    };
}
