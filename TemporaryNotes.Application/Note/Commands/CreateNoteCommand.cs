using MediatR;
using TemporaryNotes.Application.DTOs;

namespace TemporaryNotes.Application.Note.Commands
{
    public class CreateNoteCommand : IRequest<CreateNoteResponse>
    {
        public string Content { get; set; }
        public string? Password { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public int? MaxViews { get; set; }
    }
}
