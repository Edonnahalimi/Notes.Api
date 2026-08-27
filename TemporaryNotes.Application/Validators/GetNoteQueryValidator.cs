using FluentValidation;
using TemporaryNotes.Application.Note.Queries;

namespace TemporaryNotes.Application.Validators
{
    public class GetNoteQueryValidator : AbstractValidator<GetNoteQuery>
    {
        public GetNoteQueryValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Code is required.");
        }
    }
}
