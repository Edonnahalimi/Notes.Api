using FluentValidation;
using TemporaryNotes.Application.Note.Commands;

namespace TemporaryNotes.Application.Validators
{
    public class DeleteNoteCommandValidator : AbstractValidator<DeleteNoteCommand>
    {
        public DeleteNoteCommandValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Code is required.");
        }
    }
}
