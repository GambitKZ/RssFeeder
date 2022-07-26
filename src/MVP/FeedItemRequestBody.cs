using System;

namespace MVP;

public class FeedItemRequestBody
{
    public string Title { get; set; }

    public string Content { get; set; }

    public Uri Link { get; set; }
}