using MediatR;
using Microsoft.AspNetCore.Identity;
using TemporaryNotes.Application.Common.Helpers;
using TemporaryNotes.Application.DTOs;
using TemporaryNotes.Application.Interfaces;
using TemporaryNotes.Domain.Entities;

namespace TemporaryNotes.Application.Note.Commands;

public class CreateNoteCommandHandler : IRequestHandler<CreateNoteCommand, CreateNoteResponse>
{
    private readonly INoteRepository _repository;
    private readonly IPasswordHasher<Notes> _passwordHasher;

    public CreateNoteCommandHandler(
        INoteRepository repository,
        IPasswordHasher<Notes> passwordHasher)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
    }

    public async Task<CreateNoteResponse> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
    {
        var note = new Notes
        {
            Id = Guid.NewGuid(),
            Code = NoteCodeGenerator.Generate(),
            Content = request.Content,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = request.ExpiresAt,
            MaxViews = request.MaxViews,
            ViewCount = 0,
            DeletedAt = null
        };

        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            note.PasswordHash = _passwordHasher.HashPassword(note, request.Password);
        }

        await _repository.AddAsync(note, cancellationToken);

        return new CreateNoteResponse
        {
            Code = note.Code,
            Url = $"/api/notes/{note.Code}",
            ExpiresAt = note.ExpiresAt,
            MaxViews = note.MaxViews
        };
    }
}