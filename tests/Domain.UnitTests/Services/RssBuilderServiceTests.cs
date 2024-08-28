using System.ServiceModel.Syndication;
using System.Xml;
using FluentValidation;
using RssFeeder.Domain.UnitTests;

namespace RssFeeder.Domain.Services.Tests;

[TestClass()]
public class RssBuilderServiceTests
{
    private TestFeedHeader _feedHeader = null!;

    [TestInitialize]
    public void Initialize()
    {
        _feedHeader = new TestFeedHeader()
        {
            Title = "FeedTitle1",
            AlternateLink = new Uri("https://myrss.org"),
            Description = "FeedDescription1",
            Language = "en-us",
            Authors = new List<string>() { "author1" },
            Categories = new List<string>() { "category1" }
        };
    }

    [TestMethod()]
    public void NoObjectsProvided_ReceiveRssWithHeaders()
    {
        string result = RssBuilderService.GetRssStringFromItems(_feedHeader, new List<TestFeed>());

        SyndicationFeed feed = GetSyndicationFeedFromXmlString(result);

        Assert.IsTrue(feed.Title.Text.Length > 0);
        Assert.IsTrue(feed.Authors.Count > 0);
        Assert.IsFalse(feed.Items.Any());
    }

    [TestMethod()]
    public void NoObjectsProvided_ReceiveStreamRssWithHeaders()
    {
        MemoryStream result = RssBuilderService.GetRssStreamFromItems(_feedHeader, new List<TestFeed>());

        result.Position = 0;

        SyndicationFeed feed = GetSyndicationFeedFromStream(result);

        Assert.IsTrue(feed.Title.Text.Length > 0);
        Assert.IsTrue(feed.Authors.Count > 0);
        Assert.IsFalse(feed.Items.Any());
    }

    [TestMethod()]
    public void EmptyHeader_ReceiveAnException()
    {
        Assert.ThrowsException<ValidationException>(() => RssBuilderService.GetRssStringFromItems(
                        new TestFeedHeader(), new List<TestFeed>()));
    }

    [TestMethod()]
    public void EmptyHeaderForStream_ReceiveAnException()
    {
        Assert.ThrowsException<ValidationException>(() => RssBuilderService.GetRssStreamFromItems(
                        new TestFeedHeader(), new List<TestFeed>()));
    }

    [TestMethod()]
    public void OneObjectInList_ReceiveRssWithOneItem()
    {
        var testFeed = new TestFeed()
        {
            Content = "Content1",
            Id = "Id1",
            Link = "https://test1.com",
            Title = "Title1",
            Timestamp = DateTimeOffset.Now
        };

        string result = RssBuilderService.GetRssStringFromItems(_feedHeader, new List<TestFeed>() { testFeed });

        SyndicationFeed feed = GetSyndicationFeedFromXmlString(result);

        SyndicationItem feedItem = feed.Items.First();
        Assert.AreEqual(testFeed.Content, feedItem.Summary.Text);
        Assert.AreEqual(testFeed.Id, feedItem.Id);
        Assert.AreEqual(testFeed.Title, feedItem.Title.Text);
    }
    [TestMethod()]
    public void OneObjectInList_ReceiveRssStreamWithOneItem()
    {
        var testFeed = new TestFeed()
        {
            Content = "Content1",
            Id = "Id1",
            Link = "https://test1.com",
            Title = "Title1",
            Timestamp = DateTimeOffset.Now
        };

        MemoryStream result = RssBuilderService.GetRssStreamFromItems(_feedHeader, new List<TestFeed>() { testFeed });

        SyndicationFeed feed = GetSyndicationFeedFromStream(result);

        SyndicationItem feedItem = feed.Items.First();
        Assert.AreEqual(testFeed.Content, feedItem.Summary.Text);
        Assert.AreEqual(testFeed.Id, feedItem.Id);
        Assert.AreEqual(testFeed.Title, feedItem.Title.Text);
    }

    [TestMethod()]
    public void TwoObjectInList_TimestampOfLaterFeed()
    {
        var testFeed1 = new TestFeed()
        {
            Content = "Content1",
            Id = "Id1",
            Link = "https://test1.com",
            Title = "Title1",
            Timestamp = DateTimeOffset.Now.AddDays(-1)
        };
        var testFeed2 = new TestFeed()
        {
            Content = "Content2",
            Id = "Id2",
            Link = "https://test2.com",
            Title = "Title2",
            Timestamp = DateTimeOffset.Now
        };

        string result = RssBuilderService.GetRssStringFromItems(_feedHeader,
            new List<TestFeed>() { testFeed1, testFeed2 });
        SyndicationFeed feed = GetSyndicationFeedFromXmlString(result);

        Assert.AreEqual(2, feed.Items.Count());
        Assert.AreEqual(testFeed2.Timestamp.ToString(), feed.LastUpdatedTime.ToString());
    }

    [TestMethod()]
    public void TwoObjectInStreamList_TimestampOfLaterFeed()
    {
        var testFeed1 = new TestFeed()
        {
            Content = "Content1",
            Id = "Id1",
            Link = "https://test1.com",
            Title = "Title1",
            Timestamp = DateTimeOffset.Now.AddDays(-1)
        };
        var testFeed2 = new TestFeed()
        {
            Content = "Content2",
            Id = "Id2",
            Link = "https://test2.com",
            Title = "Title2",
            Timestamp = DateTimeOffset.Now
        };

        MemoryStream result = RssBuilderService.GetRssStreamFromItems(_feedHeader,
            new List<TestFeed>() { testFeed1, testFeed2 });
        SyndicationFeed feed = GetSyndicationFeedFromStream(result);

        Assert.AreEqual(2, feed.Items.Count());
        Assert.AreEqual(testFeed2.Timestamp.ToString(), feed.LastUpdatedTime.ToString());
    }

    private static SyndicationFeed GetSyndicationFeedFromXmlString(string result)
    {
        var reader = XmlReader.Create(new StringReader(result));
        var feed = SyndicationFeed.Load(reader);
        reader.Close();
        return feed;
    }

    public static SyndicationFeed GetSyndicationFeedFromStream(Stream stream)
    {
        using var xmlReader = XmlReader.Create(stream);

        return SyndicationFeed.Load(xmlReader);
    }
}