namespace TemporaryNotes.Application.DTOs;

public class GetNoteResponse
{
    public string Code { get; set; }
    public string? Content { get; set; }
    public int? RemainingViews { get; set; }
    public bool RequiresPassword { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
}