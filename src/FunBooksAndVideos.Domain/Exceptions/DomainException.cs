namespace FunBooksAndVideos.Domain.Exceptions;

/// <summary>Raised when a domain invariant would be broken.</summary>
public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }
}
