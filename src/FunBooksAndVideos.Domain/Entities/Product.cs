using FunBooksAndVideos.Domain.Enums;
using FunBooksAndVideos.Domain.Exceptions;

namespace FunBooksAndVideos.Domain.Entities;

/// <summary>
/// Base type for everything that can be put on a purchase order line.
/// New kinds of product are added by deriving from this class (Open/Closed),
/// and every subtype can be used wherever a Product is expected (Liskov).
/// </summary>
public abstract class Product
{
    protected Product(int id, string name, decimal price)
    {
        if (id <= 0)
        {
            throw new DomainException("Product id must be a positive number.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Product name is required.");
        }

        if (price < 0)
        {
            throw new DomainException("Product price cannot be negative.");
        }

        Id = id;
        Name = name;
        Price = price;
    }

    public int Id { get; }

    public string Name { get; }

    public decimal Price { get; }

    public abstract ProductType Type { get; }

    /// <summary>True when the product has to be shipped to the customer.</summary>
    public abstract bool IsPhysical { get; }
}
