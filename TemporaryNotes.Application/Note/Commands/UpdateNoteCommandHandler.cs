using MediatR;
using Microsoft.AspNetCore.Identity;
using TemporaryNotes.Application.DTOs;
using TemporaryNotes.Application.Interfaces;
using TemporaryNotes.Domain.Entities;

namespace TemporaryNotes.Application.Note.Commands;

public class UpdateNoteCommandHandler
    : IRequestHandler<UpdateNoteCommand, UpdateNoteResponse?>
{
    private readonly INoteRepository _repository;
    private readonly IPasswordHasher<Notes> _passwordHasher;

    public UpdateNoteCommandHandler(
        INoteRepository repository,
        IPasswordHasher<Notes> passwordHasher)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
    }

    public async Task<UpdateNoteResponse?> Handle(
        UpdateNoteCommand request,
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

        if (!string.IsNullOrWhiteSpace(note.PasswordHash))
        {
            if (string.IsNullOrWhiteSpace(request.Password))
                return null;

            var result = _passwordHasher.VerifyHashedPassword(
                note,
                note.PasswordHash,
                request.Password);

            if (result == PasswordVerificationResult.Failed)
                return null;
        }

        note.Content = request.Content;
        note.ExpiresAt = request.ExpiresInMinutes.HasValue
            ? DateTime.UtcNow.AddMinutes(request.ExpiresInMinutes.Value)
            : null;

        note.MaxViews = request.MaxViews;

        if (!string.IsNullOrWhiteSpace(request.NewPassword))
        {
            note.PasswordHash = _passwordHasher.HashPassword(
                note,
                request.NewPassword);
        }

        await _repository.UpdateAsync(
            note,
            cancellationToken);

        return new UpdateNoteResponse
        {
            Code = note.Code,
            ExpiresAt = note.ExpiresAt,
            MaxViews = note.MaxViews
        };
    }
}