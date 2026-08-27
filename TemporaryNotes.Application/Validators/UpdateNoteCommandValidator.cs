using FluentValidation;
using TemporaryNotes.Application.Note.Commands;

namespace TemporaryNotes.Application.Validators
{
    public class UpdateNoteCommandValidator : AbstractValidator<UpdateNoteCommand>
    {
        public UpdateNoteCommandValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("Content is required.")
                .Must(content => !string.IsNullOrWhiteSpace(content))
                .WithMessage("Content cannot be empty or whitespace.")
                .MaximumLength(1000)
                .WithMessage("Content cannot exceed 1000 characters.");

            RuleFor(x => x.ExpiresInMinutes)
                .GreaterThan(0)
                .When(x => x.ExpiresInMinutes.HasValue)
                .WithMessage("Expiration must be greater than 0 minutes.");

            RuleFor(x => x.MaxViews)
                .GreaterThan(0)
                .When(x => x.MaxViews.HasValue)
                .WithMessage("MaxViews must be greater than 0.");

            RuleFor(x => x.NewPassword)
                .MinimumLength(6)
                .When(x => !string.IsNullOrWhiteSpace(x.NewPassword))
                .WithMessage("New password must be at least 6 characters.");
        }
    }
}