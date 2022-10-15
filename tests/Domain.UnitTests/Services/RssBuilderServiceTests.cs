using System.ServiceModel.Syndication;
using System.Xml;
using FluentValidation;
using RssFeeder.Domain.Entities;
using RssFeeder.Domain.UnitTests;

namespace RssFeeder.Domain.Services.Tests;

[TestClass()]
public class RssBuilderServiceTests
{
    private FeedHeader feedHeader;

    [TestInitialize]
    public void Initialize()
    {
        feedHeader = new FeedHeader()
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
        var result = RssBuilderService.GetRssStringFromItems(feedHeader, new List<TestFeed>());

        XmlReader reader = XmlReader.Create(new StringReader(result));
        SyndicationFeed feed = SyndicationFeed.Load(reader);
        reader.Close();

        RssHasHeaders(feed);
        Assert.IsFalse(feed.Items.Any());
    }

    [TestMethod()]
    public void EmptyHeader_ReceiveAnException()
    {
        Assert.ThrowsException<ValidationException>(() => RssBuilderService.GetRssStringFromItems(
                        new FeedHeader(), new List<TestFeed>()));
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

        var result = RssBuilderService.GetRssStringFromItems(feedHeader, new List<TestFeed>() { testFeed });

        XmlReader reader = XmlReader.Create(new StringReader(result));
        SyndicationFeed feed = SyndicationFeed.Load(reader);
        reader.Close();

        RssHasHeaders(feed);
        Assert.IsTrue(feed.Items.Any());

        var feedItem = feed.Items.First();
        Assert.AreEqual(testFeed.Content, feedItem.Summary.Text);
        Assert.AreEqual(testFeed.Id, feedItem.Id);
        Assert.AreEqual(testFeed.Title, feedItem.Title.Text);
    }

    private static void RssHasHeaders(SyndicationFeed feed)
    {
        Assert.IsTrue(feed.Title.Text.Length > 0);
        Assert.IsTrue(feed.Authors.Count > 0);
    }
}