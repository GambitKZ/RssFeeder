using FluentValidation;
using RssFeeder.Application.Common.Models;
using RssFeeder.Application.FeedItem.Commands.CreateFeedItem;
using RssFeeder.Application.Validators;

namespace RssFeeder.Application.UnitTests.FeedItem.Commands.CreateFeedItem;

[TestClass()]
public class CreateFeedItemsCommandTests
{
    private CreateFeedItemsCommandValidator _validator = null!;

    [TestInitialize]
    public void Initialize()
    {
        _validator = new CreateFeedItemsCommandValidator();
    }

    [TestMethod()]
    public void ProvideEmptyList_ReceiveAnError()
    {
        var createFeedCommand = new CreateFeedItemsCommand(new List<UploadFeedItem>());

        void Action() => _validator.ValidateAndThrow(createFeedCommand);

        _ = Assert.ThrowsException<ValidationException>(Action);
    }

    [TestMethod()]
    public void ProvideIncorrectObject_ReceiveAnError()
    {
        var incorrectFeedItem = new UploadFeedItem()
        {
            Title = "Title1",
            Content = "",
            Link = null
        };
        var createFeedCommand = new CreateFeedItemsCommand(
                    new List<UploadFeedItem>() { incorrectFeedItem });

        void Action() => _validator.ValidateAndThrow(createFeedCommand);

        _ = Assert.ThrowsException<ValidationException>(Action);
    }

    [TestMethod()]
    public void CorrectObject_ValidationPass()
    {
        var correctFeedItem = new UploadFeedItem()
        {
            Title = "Title1",
            Content = "Content1",
            Link = "Link1"
        };
        var createFeedCommand = new CreateFeedItemsCommand(
                    new List<UploadFeedItem>() { correctFeedItem });

        var result = _validator.Validate(createFeedCommand);

        Assert.IsTrue(result.IsValid);
    }
}