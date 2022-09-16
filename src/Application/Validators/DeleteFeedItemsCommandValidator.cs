using FluentValidation;
using RssFeeder.Application.FeedItem.Commands.DeleteFeedItems;

namespace RssFeeder.Application.Validators;

public class DeleteFeedItemsCommandValidator : AbstractValidator<DeleteFeedItemsCommand>
{
    public DeleteFeedItemsCommandValidator()
    {
        RuleFor(list => list.ListOfFeedId).NotNull().Must(x => x.Count > 0).WithMessage("List with Id should not be Empty");

        RuleForEach(list => list.ListOfFeedId).Must(BeValidGuid).WithMessage("List should contain valid GUIDs");
    }

    private bool BeValidGuid(string id)
    {
        return Guid.TryParse(id, out Guid _);
    }
}