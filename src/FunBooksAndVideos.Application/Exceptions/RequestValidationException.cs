namespace FunBooksAndVideos.Application.Exceptions;

public sealed class RequestValidationException : Exception
{
    public RequestValidationException(IReadOnlyDictionary<string, string[]> errors)
        : base("One or more validation errors occurred.")
    {
        Errors = errors;
    }

    public RequestValidationException(string field, string error)
        : this(new Dictionary<string, string[]> { [field] = new[] { error } })
    {
    }

    public IReadOnlyDictionary<string, string[]> Errors { get; }
}
