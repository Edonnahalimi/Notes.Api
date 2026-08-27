using MediatR;
using Microsoft.AspNetCore.Identity;
using TemporaryNotes.Application.Interfaces;
using TemporaryNotes.Domain.Entities;

namespace TemporaryNotes.Application.Note.Commands;

public class DeleteNoteCommandHandler : IRequestHandler<DeleteNoteCommand, bool>
{
    private readonly INoteRepository _repository;
    private readonly IPasswordHasher<Notes> _passwordHasher;

    public DeleteNoteCommandHandler(
        INoteRepository repository,
        IPasswordHasher<Notes> passwordHasher)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
    }

    public async Task<bool> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
    {
        var note = await _repository.GetByCodeAsync(
            request.Code,
            cancellationToken);

        if (note is null || note.DeletedAt.HasValue)
            return false;

        if (!string.IsNullOrWhiteSpace(note.PasswordHash))
        {
            if (string.IsNullOrWhiteSpace(request.Password))
                return false;

            var result = _passwordHasher.VerifyHashedPassword(
                note,
                note.PasswordHash,
                request.Password);

            if (result == PasswordVerificationResult.Failed)
                return false;
        }

        note.DeletedAt = DateTime.UtcNow;

        await _repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}