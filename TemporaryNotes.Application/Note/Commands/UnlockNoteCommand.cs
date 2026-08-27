using MediatR;
using TemporaryNotes.Application.DTOs;

namespace TemporaryNotes.Application.Note.Commands
{
    public class UnlockNoteCommand : IRequest<GetNoteResponse?>
    {
        public string Code { get; set; }
        public string Password { get; set; }
    }
}
