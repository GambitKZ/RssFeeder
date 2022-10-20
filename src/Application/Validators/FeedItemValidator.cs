using FluentValidation;
using RssFeeder.SharedKernel.Interfaces;

namespace RssFeeder.Application.Validators;

// TODO: Remove if unused
public class FeedItemValidator : AbstractValidator<IFeedItem>
{
    public FeedItemValidator()
    {
        const string message = "{PropertyName} of the Feed Item should Not be Empty or Null";

        _ = RuleFor(x => x.Title).NotNull().NotEmpty().WithMessage(message);
        _ = RuleFor(x => x.Content).NotNull().NotEmpty().WithMessage(message);
        _ = RuleFor(x => x.Link).NotNull().NotEmpty().WithMessage(message);
    }
}