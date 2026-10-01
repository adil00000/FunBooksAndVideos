using FunBooksAndVideos.Domain.Enums;

namespace FunBooksAndVideos.Domain.Entities;

/// <summary>A physical book that is shipped to the customer.</summary>
public sealed class Book : Product
{
    public Book(int id, string name, decimal price, string author)
        : base(id, name, price)
    {
        Author = author ?? string.Empty;
    }

    public string Author { get; }

    public override ProductType Type => ProductType.Book;

    public override bool IsPhysical => true;
}
