using FluentValidation;
using RssFeeder.SharedKernel.Interfaces;

namespace RssFeeder.Application.Validators;

public class FeedItemValidator : AbstractValidator<IFeedItem>
{
    public FeedItemValidator()
    {
        RuleFor(x => x.Title).NotNull()
                                 .NotEmpty()
                                 .WithMessage("Title of the Feed Item should Not be Empty or Null");
        RuleFor(x => x.Content).NotNull()
                                   .NotEmpty()
                                   .WithMessage("Content of the Feed Item should Not be Empty or Null");
        RuleFor(x => x.Link).NotNull()
                                .NotEmpty()
                                .WithMessage("Link of the Feed Item should Not be Empty or Null");
    }
}