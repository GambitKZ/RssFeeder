using RssFeeder.Domain.Interfaces;

namespace RssFeeder.Domain.Entities;

// TODO: Remove from Domain.
//       Create one in Test, another in Application
public class FeedHeader : IFeedHeader
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public Uri? AlternateLink { get; set; }
    public IList<string>? Authors { get; set; }
    public IList<string>? Categories { get; set; }
    public string? Language { get; set; }
}