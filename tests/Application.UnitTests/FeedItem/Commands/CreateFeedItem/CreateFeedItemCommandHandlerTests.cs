using AutoMapper;
using Moq;
using RssFeeder.Application.Common.Models;
using RssFeeder.SharedKernel.Interfaces;

namespace RssFeeder.Application.FeedItem.Commands.CreateFeedItem.Tests;

[TestClass()]
public class CreateFeedItemCommandHandlerTests
{
    private Mock<IMapper> _mapper = null!;
    private Mock<IRepositoryBase<IFeedItem>> _repository = null!;

    [TestInitialize]
    public void Initialize()
    {
        _repository = new Mock<IRepositoryBase<IFeedItem>>();
        _mapper = new Mock<IMapper>();
        _mapper.Setup(m => m.Map<IList<UploadFeedItem>,
                    IEnumerable<FeedItemRepositoryDto>>(It.IsAny<IList<UploadFeedItem>>()))
            .Returns(new List<FeedItemRepositoryDto>());
    }

    [TestMethod()]
    public async Task CreateFeedItemCommandHandler_ReceiveFeedsForSave_OrderOfProcessIsCorrect()
    {
        int eventOrder = 0;

        _repository.Setup(rep => rep.AddRange(It.IsAny<IEnumerable<IFeedItem>>()))
                .Callback(() => Assert.AreEqual(0, eventOrder++));
        _repository.Setup(rep => rep.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Callback(() => Assert.AreEqual(1, eventOrder++));

        var createFeedItemsCommand = new CreateFeedItemsCommand(new List<UploadFeedItem>());

        var feederCommand = new CreateFeedItemCommandHandler(_repository.Object, _mapper.Object);

        await feederCommand.Handle(createFeedItemsCommand, new CancellationToken());
    }
}