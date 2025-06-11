using System.Xml.Linq;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using RssFeeder.Application.RssFeed.Queries.GetRss;
using RssFeeder.Domain.Interfaces;

namespace RssFeeder.Application.UnitTests.RssFeed.Queries.GetRss;

[TestClass]
public class GetRssFeedStreamTests
{
    private readonly Mock<IRepositoryBase<IFeedItem>> _mockRepository;
    private readonly GetRssFeedStreamHandler _handler;
    private readonly IFixture _fixture;

    public GetRssFeedStreamTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
        _mockRepository = new Mock<IRepositoryBase<IFeedItem>>();
        _handler = new GetRssFeedStreamHandler(_mockRepository.Object);
    }

    [TestMethod]
    public async Task Handle_ShouldReturnCorrectRssStream_WhenDataExists()
    {
        // Arrange
        const int numberOfElements = 3;

        var feedItems = _fixture.Build<FeedItemTest>()
            .With(x => x.Link, _fixture.Create<Uri>().ToString())
            .CreateMany(numberOfElements).ToList();
        _mockRepository.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
                       .ReturnsAsync(feedItems);

        // Act
        MemoryStream result = await _handler.Handle(new GetRssFeedStream(), CancellationToken.None);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(MemoryStream));
        _mockRepository.Verify(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        AssertItemsCount(numberOfElements, result);
    }

    [TestMethod]
    public async Task Handle_ShouldReturnRssStreamWithoutData_WhenDataIsNotExists()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
                       .ReturnsAsync(Enumerable.Empty<IFeedItem>());

        // Act
        MemoryStream result = await _handler.Handle(new GetRssFeedStream(), CancellationToken.None);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(MemoryStream));
        _mockRepository.Verify(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        AssertItemsCount(0, result);
    }

    private static void AssertItemsCount(int numberOfElements, MemoryStream result)
    {
        result.Position = 0;
        using var reader = new StreamReader(result);
        string content = reader.ReadToEnd();
        var doc = XDocument.Parse(content);
        IEnumerable<XElement> items = doc.Descendants("channel").Descendants("item");
        Assert.AreEqual(numberOfElements, items.Count());
    }

    private class FeedItemTest : IFeedItem
    {
        // Implement the properties here
        public string? Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Link { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
    }
}