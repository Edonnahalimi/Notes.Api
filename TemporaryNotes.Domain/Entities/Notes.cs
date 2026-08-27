namespace TemporaryNotes.Domain.Entities
{
    public class Notes
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Content { get; set; }
        public string? PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public int? MaxViews { get; set; }
        public int ViewCount { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
