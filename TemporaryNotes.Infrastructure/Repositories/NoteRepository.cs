using Microsoft.EntityFrameworkCore;
using TemporaryNotes.Application.Interfaces;
using TemporaryNotes.Domain.Entities;

namespace TemporaryNotes.Infrastructure.Repositories;

public class NoteRepository : INoteRepository
{
    private readonly AppDbContext _context;

    public NoteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Notes note,
        CancellationToken cancellationToken)
    {
        await _context.Notes.AddAsync(note, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task<Notes?> GetByCodeAsync(
        string code,
        CancellationToken cancellationToken)
    {
        return _context.Notes
            .FirstOrDefaultAsync(
                note => note.Code == code,
                cancellationToken);
    }

    public async Task UpdateAsync(
        Notes note,
        CancellationToken cancellationToken)
    {
        _context.Notes.Update(note);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task SaveChangesAsync(
        CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> IncrementViewCountAsync(
        Guid noteId,
        int? maxViews,
        CancellationToken cancellationToken)
    {
        var query = _context.Notes
            .Where(note => note.Id == noteId);

        if (maxViews.HasValue)
        {
            query = query.Where(
                note => note.ViewCount < maxViews.Value);
        }

        var affectedRows = await query.ExecuteUpdateAsync(
            setters => setters.SetProperty(
                note => note.ViewCount,
                note => note.ViewCount + 1),
            cancellationToken);

        return affectedRows > 0;
    }
}