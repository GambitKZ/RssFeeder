using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Moq;
using RssFeeder.Application.Common.Mappings;
using RssFeeder.Application.Common.Models;
using RssFeeder.Domain.Interfaces;

namespace RssFeeder.Application.FeedItem.Commands.CreateFeedItem.Tests;

[TestClass()]
public class CreateFeedItemCommandHandlerTests
{
    private IFixture _fixture = null!;

    private IMapper _mapper = null!;
    private Mock<IRepositoryBase<IFeedItem>> _repository = null!;

    [TestInitialize]
    public void Initialize()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());

        _repository = new Mock<IRepositoryBase<IFeedItem>>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<UploadToRepositoryFeedItemMapperProfile>();
        });
        _mapper = config.CreateMapper();
    }

    [TestMethod()]
    public async Task ReceiveFeedsForSave_OrderOfProcessIsCorrect()
    {
        int eventOrder = 0;

        _repository.Setup(rep => rep.AddRange(It.IsAny<IEnumerable<IFeedItem>>()))
                .Callback(() => Assert.AreEqual(0, eventOrder++));
        _repository.Setup(rep => rep.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Callback(() => Assert.AreEqual(1, eventOrder++));

        var feedItems = _fixture.CreateMany<UploadFeedItem>();

        var createFeedItemsCommand = new CreateFeedItemsCommand(new List<UploadFeedItem>());

        var feederCommand = new CreateFeedItemCommandHandler(_repository.Object, _mapper);

        await feederCommand.Handle(createFeedItemsCommand, new CancellationToken());
    }

    [TestMethod()]
    public async Task ReceiveFeedsForSave_FeedsSaved()
    {
        // Arrange
        var feedItems = _fixture.CreateMany<UploadFeedItem>();
        var createFeedItemsCommand = new CreateFeedItemsCommand(feedItems.ToList());

        _repository.Setup(rep => rep.AddRange(It.IsAny<IEnumerable<IFeedItem>>()));
        _repository.Setup(rep => rep.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => feedItems.Count());

        var feederCommand = new CreateFeedItemCommandHandler(_repository.Object, _mapper);

        // Act
        var response = await feederCommand.Handle(createFeedItemsCommand, new CancellationToken());

        // Assert
        _repository.Verify(rep => rep.AddRange(It.Is<IEnumerable<IFeedItem>>(items => items.Count() == feedItems.Count())), Times.Once);
        Assert.AreEqual("Feed was saved", response);
    }
}