using FluentValidation;
using RssFeeder.Application.Common.Models;

namespace RssFeeder.Application.Validators;

public class UploadFeedItemValidator : AbstractValidator<UploadFeedItem>
{
    public UploadFeedItemValidator()
    {
        var message = "{PropertyName} of the Feed Item should Not be Empty or Null";

        RuleFor(x => x.Title).NotNull()
                                 .NotEmpty()
                                 .WithMessage(message);
        RuleFor(x => x.Content).NotNull()
                                   .NotEmpty()
                                   .WithMessage(message);
        RuleFor(x => x.Link).NotNull()
                                .NotEmpty()
                                .WithMessage(message);
    }
}