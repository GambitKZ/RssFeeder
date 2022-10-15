using FluentValidation;
using RssFeeder.Domain.Interfaces;

namespace RssFeeder.Domain.Validators;

public class RssHeaderValidator : AbstractValidator<IFeedHeader>
{
    public RssHeaderValidator()
    {
        const string message = "{PropertyName} of the Feed's Header should Not be Empty or Null";

        RuleFor(fh => fh.Title).NotNull()
                                .NotEmpty()
                                .WithMessage(message);
        RuleFor(fh => fh.Description).NotNull()
                                .NotEmpty()
                                .WithMessage(message);
        RuleFor(fh => fh.AlternateLink).NotNull()
                                .NotEmpty()
                                .WithMessage(message);
        RuleFor(fh => fh.Language).NotNull()
                                .NotEmpty()
                                .WithMessage(message);

        RuleFor(fh => fh.Categories).NotNull()
                                .NotEmpty()
                                .WithMessage(message);

        RuleFor(fh => fh.Authors).NotNull().Must(authors => authors?.Count > 0).WithMessage(message)
            .ForEach(author => author.NotNull().NotEmpty()
                .WithMessage("Author should Not be Empty or Null"));

        RuleFor(fh => fh.Categories).NotNull().Must(categories => categories?.Count > 0).WithMessage(message)
            .ForEach(category => category.NotNull().NotEmpty()
                .WithMessage("Category should Not be Empty or Null"));
    }
}