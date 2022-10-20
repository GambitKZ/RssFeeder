using FluentValidation;
using RssFeeder.Application.Common.Models;

namespace RssFeeder.Application.Validators;

public class UploadFeedItemValidator : AbstractValidator<UploadFeedItem>
{
    public UploadFeedItemValidator()
    {
        const string message = "{PropertyName} of the Feed Item should Not be Empty or Null";

        _ = RuleFor(x => x.Title).NotNull()
                                 .NotEmpty()
                                 .WithMessage(message);
        _ = RuleFor(x => x.Content).NotNull()
                                   .NotEmpty()
                                   .WithMessage(message);
        _ = RuleFor(x => x.Link).NotNull()
                                .NotEmpty()
                                .WithMessage(message);
    }
}