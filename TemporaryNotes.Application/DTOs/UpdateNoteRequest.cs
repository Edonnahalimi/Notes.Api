namespace TemporaryNotes.Application.DTOs
{
    public class UpdateNoteRequest
    {
        public string Content { get; set; }
        public int? ExpiresInMinutes { get; set; }
        public int? MaxViews { get; set; }
        public string? Password { get; set; }
        public string? NewPassword { get; set; }
    }
}
