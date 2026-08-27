using MediatR;

namespace TemporaryNotes.Application.Note.Commands
{
    public class DeleteNoteCommand : IRequest<bool>
    {
        public string Code { get; set; }
        public string? Password { get; set; }
    }
}
