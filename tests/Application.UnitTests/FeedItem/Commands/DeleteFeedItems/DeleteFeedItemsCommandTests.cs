using FluentValidation;
using RssFeeder.Application.Validators;

namespace RssFeeder.Application.FeedItem.Commands.DeleteFeedItems.Tests;

[TestClass()]
public class DeleteFeedItemsCommandTests
{
    private DeleteFeedItemsCommandValidator _validator = null!;

    [TestInitialize]
    public void Initialize()
    {
        _validator = new DeleteFeedItemsCommandValidator();
    }

    [TestMethod()]
    public void EmptyList_ReceiveAnException()
    {
        var deleteFeedItemsCommand = new DeleteFeedItemsCommand(new List<string>());

        void Action() => _validator.ValidateAndThrow(deleteFeedItemsCommand);

        _ = Assert.ThrowsException<ValidationException>(Action);
    }

    [TestMethod()]
    public void ElementIsNotGuid_ReceiveAnException()
    {
        var deleteFeedItemsCommand = new DeleteFeedItemsCommand(
                        new List<string>() { "NotGuid" });

        void Action() => _validator.ValidateAndThrow(deleteFeedItemsCommand);

        _ = Assert.ThrowsException<ValidationException>(Action);
    }

    [TestMethod()]
    public void ElementIsCorrectGuid_ReceiveAnException()
    {
        var deleteFeedItemsCommand = new DeleteFeedItemsCommand(
                      new List<string>() { Guid.NewGuid().ToString() });

        var result = _validator.Validate(deleteFeedItemsCommand);

        Assert.IsTrue(result.IsValid);
    }
}