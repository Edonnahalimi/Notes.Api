using MediatR;
using TemporaryNotes.Application.DTOs;
using TemporaryNotes.Application.Interfaces;

namespace TemporaryNotes.Application.Note.Queries;

public class GetNoteQueryHandler
    : IRequestHandler<GetNoteQuery, GetNoteResponse?>
{
    private readonly INoteRepository _repository;

    public GetNoteQueryHandler(INoteRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetNoteResponse?> Handle(
        GetNoteQuery request,
        CancellationToken cancellationToken)
    {
        var note = await _repository.GetByCodeAsync(
            request.Code,
            cancellationToken);

        if (note is null)
            return null;

        if (note.DeletedAt.HasValue)
            return null;

        if (note.ExpiresAt.HasValue &&
            note.ExpiresAt.Value <= DateTime.UtcNow)
        {
            return null;
        }

        // Password-protected note:
        // do not expose content.
        if (!string.IsNullOrWhiteSpace(note.PasswordHash))
        {
            return new GetNoteResponse
            {
                Code = note.Code,
                Content = null,
                RemainingViews = null,
                RequiresPassword = true,
                CreatedAt = note.CreatedAt,
                ExpiresAt = note.ExpiresAt
            };
        }

        if (note.MaxViews.HasValue &&
            note.ViewCount >= note.MaxViews.Value)
        {
            return null;
        }

        var incremented =
            await _repository.IncrementViewCountAsync(
                note.Id,
                note.MaxViews,
                cancellationToken);

        if (!incremented)
            return null;

        return new GetNoteResponse
        {
            Code = note.Code,
            Content = note.Content,
            RemainingViews = note.MaxViews.HasValue
                ? note.MaxViews.Value - note.ViewCount
                : null,
            RequiresPassword = false,
            CreatedAt = note.CreatedAt,
            ExpiresAt = note.ExpiresAt
        };
    }
}