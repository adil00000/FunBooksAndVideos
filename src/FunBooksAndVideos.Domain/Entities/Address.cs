namespace FunBooksAndVideos.Domain.Entities;

/// <summary>Postal address value object.</summary>
public sealed record Address(string Line1, string City, string PostCode, string Country);
