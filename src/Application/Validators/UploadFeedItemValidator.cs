using FluentValidation;
using RssFeeder.Application.Common.Models;

namespace RssFeeder.Application.Validators;

public class UploadFeedItemValidator : AbstractValidator<UploadFeedItem>
{
    public UploadFeedItemValidator()
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