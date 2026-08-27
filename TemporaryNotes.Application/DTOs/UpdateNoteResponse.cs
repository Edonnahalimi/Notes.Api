namespace TemporaryNotes.Application.DTOs
{
    public class UpdateNoteResponse
    {
        public string Code { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public int? MaxViews { get; set; }
    }
}
