using FluentValidation;

namespace TemporaryNotes.Application.Note.Commands
{
    public class CreateNoteCommandValidator : AbstractValidator<CreateNoteCommand>
    {
        public CreateNoteCommandValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("Content is required.")
                .MaximumLength(1000)
                .WithMessage("Content cannot exceed 1000 characters.");

            RuleFor(x => x.Password)
                .MaximumLength(100)
                .WithMessage("Password cannot exceed 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Password));

            RuleFor(x => x.ExpiresAt)
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("ExpiresAt must be in the future.")
                .When(x => x.ExpiresAt.HasValue);

            RuleFor(x => x.MaxViews)
                .GreaterThan(0)
                .WithMessage("MaxViews must be greater than 0.")
                .When(x => x.MaxViews.HasValue);
        }
    }
}