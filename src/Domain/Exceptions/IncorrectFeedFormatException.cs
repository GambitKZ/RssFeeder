namespace RssFeeder.Domain.Exceptions;

public class IncorrectFeedFormatException : Exception
{
    public IncorrectFeedFormatException(string? message) :
        base($"Can't parse the Message, got the {message} error")
    {
    }
}