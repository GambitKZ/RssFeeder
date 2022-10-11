using System.ServiceModel.Syndication;
using System.Xml;
using RssFeeder.Domain.UnitTests;

namespace RssFeeder.Domain.Services.Tests;

[TestClass()]
public class RssBuilderServiceTests
{
    [TestMethod()]
    public void NoObjectsProvided_ReceiveRssWithHeaders()
    {
        var result = RssBuilderService.GetRssStringFromItems(new List<TestFeed>());

        XmlReader reader = XmlReader.Create(new StringReader(result));
        SyndicationFeed feed = SyndicationFeed.Load(reader);
        reader.Close();

        RssHasHeaders(feed);

        Assert.IsFalse(feed.Items.Any());
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

        var result = RssBuilderService.GetRssStringFromItems(new List<TestFeed>() { testFeed });

        XmlReader reader = XmlReader.Create(new StringReader(result));
        SyndicationFeed feed = SyndicationFeed.Load(reader);
        reader.Close();

        RssHasHeaders(feed);

        Assert.IsTrue(feed.Items.Any());
    }

    private static void RssHasHeaders(SyndicationFeed feed)
    {
        Assert.IsTrue(feed.Title.Text.Length > 0);
        Assert.IsTrue(feed.Authors.Count > 0);
    }
}