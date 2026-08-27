using MediatR;
using TemporaryNotes.Application.DTOs;

public class UpdateNoteCommand : IRequest<UpdateNoteResponse?>
{
    public string Content { get; set; } 
    public int? ExpiresInMinutes { get; set; }
    public int? MaxViews { get; set; }
    public string? Password { get; set; }
    public string? NewPassword { get; set; }
    public string? Code { get; set; } 
}