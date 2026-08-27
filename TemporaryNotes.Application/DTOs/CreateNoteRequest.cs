namespace TemporaryNotes.Application.DTOs
{
    public class CreateNoteRequest
    {
        public string Content { get; set; } 
        public int? ExpiresAt { get; set; }
        public int? MaxViews { get; set; }
        public string? Password { get; set; }
    }
}
