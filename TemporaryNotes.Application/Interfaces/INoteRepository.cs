using TemporaryNotes.Domain.Entities;

namespace TemporaryNotes.Application.Interfaces
{
    public interface INoteRepository
    {
        Task AddAsync(Notes note, CancellationToken cancellationToken);
        Task<Notes?> GetByCodeAsync(string code, CancellationToken cancellationToken);
        Task UpdateAsync(Notes note, CancellationToken cancellationToken);
        Task SaveChangesAsync(CancellationToken cancellationToken);
        Task<bool> IncrementViewCountAsync(Guid noteId, int? maxViews, CancellationToken cancellationToken);
    }
}
