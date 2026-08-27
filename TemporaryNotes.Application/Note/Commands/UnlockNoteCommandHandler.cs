using MediatR;
using Microsoft.AspNetCore.Identity;
using TemporaryNotes.Application.DTOs;
using TemporaryNotes.Application.Interfaces;
using TemporaryNotes.Domain.Entities;

namespace TemporaryNotes.Application.Note.Commands;

public class UnlockNoteCommandHandler : IRequestHandler<UnlockNoteCommand, GetNoteResponse?>
{
    private readonly INoteRepository _repository;
    private readonly IPasswordHasher<Notes> _passwordHasher;

    public UnlockNoteCommandHandler(
        INoteRepository repository,
        IPasswordHasher<Notes> passwordHasher)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
    }

    public async Task<GetNoteResponse?> Handle(
        UnlockNoteCommand request,
        CancellationToken cancellationToken)
    {
        var note = await _repository.GetByCodeAsync(
            request.Code,
            cancellationToken);

        if (note is null ||
            note.DeletedAt.HasValue ||
            (note.ExpiresAt.HasValue &&
             note.ExpiresAt.Value <= DateTime.UtcNow))
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(note.PasswordHash) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return null;
        }

        var verificationResult = _passwordHasher.VerifyHashedPassword(
            note,
            note.PasswordHash,
            request.Password);

        if (verificationResult == PasswordVerificationResult.Failed)
            return null;

        if (note.MaxViews.HasValue &&
            note.ViewCount >= note.MaxViews.Value)
        {
            return null;
        }

        var incremented = await _repository.IncrementViewCountAsync(
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