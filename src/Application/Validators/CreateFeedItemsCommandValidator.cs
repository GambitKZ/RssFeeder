using FluentValidation;
using RssFeeder.Application.FeedItem.Commands.CreateFeedItem;

namespace RssFeeder.Application.Validators;

public class CreateFeedItemsCommandValidator : AbstractValidator<CreateFeedItemsCommand>
{
    public CreateFeedItemsCommandValidator()
    {
        RuleFor(list => list.ListOfFeeds).Must(x => x.Count > 0).WithMessage("List of Feeds should not be Empty");
        RuleForEach(list => list.ListOfFeeds).SetValidator(new UploadFeedItemValidator());
    }
}