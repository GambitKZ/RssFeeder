using Moq;
using RssFeeder.SharedKernel.Interfaces;

namespace RssFeeder.Application.FeedItem.Commands.DeleteFeedItems.Tests;

[TestClass()]
public class DeleteFeedItemsCommandHandlerTests
{
    private Mock<IRepositoryBase<IFeedItem>> _repository = null!;

    [TestInitialize]
    public void Initialize()
    {
        _repository = new Mock<IRepositoryBase<IFeedItem>>();
    }

    [TestMethod()]
    public async Task DeleteFeedItemsCommandHandler_ReceiveIdsForRemoval_OrderOfProcessIsCorrect()
    {
        int eventOrder = 0;

        _repository.Setup(rep => rep.DeleteRange(It.IsAny<IList<string>>()))
                .Callback(() => Assert.AreEqual(0, eventOrder++));
        _repository.Setup(rep => rep.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Callback(() => Assert.AreEqual(1, eventOrder++));

        var deleteFeedItemsCommand = new DeleteFeedItemsCommand(new List<string>());

        var deleteFeedItemsCommandHandler = new DeleteFeedItemsCommandHandler(_repository.Object);

        await deleteFeedItemsCommandHandler.Handle(deleteFeedItemsCommand, new CancellationToken());
    }
}