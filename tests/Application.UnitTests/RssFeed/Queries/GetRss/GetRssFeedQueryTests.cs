using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using RssFeeder.Application.RssFeed.Queries.GetRss;
using RssFeeder.Domain.Interfaces;

namespace RssFeeder.Application.UnitTests.RssFeed.Queries.GetRss;

//[TestClass]
public class GetRssFeedQueryTests
{
    private readonly Mock<IRepositoryBase<IFeedItem>> _mockRepository;
    private readonly GetRssFeedQueryHandler _handler;
    private readonly Fixture _fixture;

    public GetRssFeedQueryTests()
    {
        _fixture = new Fixture();
        _mockRepository = new Mock<IRepositoryBase<IFeedItem>>();

        // Auto setup all interfaces with Moq using AutoFixture
        _fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });

        // instantiate the handler with the mocked repository
        _handler = new GetRssFeedQueryHandler(_mockRepository.Object);
    }

    [TestMethod]
    public async Task Handle_ShouldReturnCorrectRssString_WhenDataExists()
    {
        // Arrange
        List<IFeedItem> feedItems = _fixture.Create<List<IFeedItem>>();
        _mockRepository.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
                       .ReturnsAsync(feedItems);

        // creating expected RSS string can be complex depending on RssBuilderService details
        string expectedRssString = "expected RSS string based on given feedItems";

        // Mock the RssBuilderService static call if possible or adjust design for testability
        // Assuming RssBuilderService.GetRssStringFromItems is testable or mocked indirectly

        // Act
        string result = await _handler.Handle(new GetRssFeedQuery(), CancellationToken.None);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(string));
        Assert.AreEqual(expectedRssString, result);

        // Verify repository methods were called
        _mockRepository.Verify(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task Handle_ShouldHandleEmptyData_WithoutThrowing()
    {
        // Arrange
        var feedItems = new List<IFeedItem>();
        _mockRepository.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
                       .ReturnsAsync(feedItems);

        // RssBuilderService should handle empty lists and return a valid RSS header
        string expectedEmptyRss = "<rss><channel><title>Gambit's Personal RSS</title></channel></rss>";

        // Act
        string result = await _handler.Handle(new GetRssFeedQuery(), CancellationToken.None);

        // Assert
        Assert.AreEqual(expectedEmptyRss, result);
    }
}