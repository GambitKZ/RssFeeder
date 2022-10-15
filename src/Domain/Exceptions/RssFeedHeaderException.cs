namespace RssFeeder.Domain.Exceptions;

public class RssFeedHeaderException : Exception
{
    // TODO: Remove if cannot be placed into FluentValidator
    public RssFeedHeaderException(string? message) :
        base($"RSS Header is incorrect, failed with {message} error")
    {
    }
}