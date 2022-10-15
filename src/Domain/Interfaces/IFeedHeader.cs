namespace RssFeeder.Domain.Interfaces;

public interface IFeedHeader
{
    string? Title { get; set; }

    string? Description { get; set; }

    Uri? AlternateLink { get; set; }

    IList<string>? Authors { get; set; }

    IList<string>? Categories { get; set; }

    string? Language { get; set; }
}