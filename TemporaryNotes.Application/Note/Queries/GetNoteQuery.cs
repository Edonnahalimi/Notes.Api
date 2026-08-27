using MediatR;
using TemporaryNotes.Application.DTOs;

namespace TemporaryNotes.Application.Note.Queries
{
    public class GetNoteQuery : IRequest<GetNoteResponse?>
    {
        public string Code { get; set; }
    }
}