using FluentValidation;
using RssFeeder.Application.Validators;

namespace RssFeeder.Application.FeedItem.Commands.CreateFeedItem;

public class CreateFeedItemsCommandValidator : AbstractValidator<CreateFeedItemsCommand>
{
    public CreateFeedItemsCommandValidator()
    {
        RuleFor(list => list.ListOfFeeds).Must(x => x.Count > 0).WithMessage("List of Feeds should not be Empty");
        RuleForEach(list => list.ListOfFeeds).SetValidator(new FeedItemValidator());
    }
}