namespace TemporaryNotes.Application.DTOs
{
    public class CreateNoteResponse
    {
        public string Code { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public DateTime? ExpiresAt { get; set; }
        public int? MaxViews { get; set; }
    }
}
