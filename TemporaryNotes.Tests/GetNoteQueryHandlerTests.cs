using Moq;
using TemporaryNotes.Application.Interfaces;
using TemporaryNotes.Application.Note.Queries;
using TemporaryNotes.Domain.Entities;
using Xunit;

namespace TemporaryNotes.Tests;

public class GetNoteQueryHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsNote_WhenNoteIsValid()
    {
        var note = new Notes
        {
            Id = Guid.NewGuid(),
            Code = "abc123",
            Content = "Test note",
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            MaxViews = 5,
            ViewCount = 0
        };

        var repository = new Mock<INoteRepository>();

        repository
            .Setup(x => x.GetByCodeAsync(
                note.Code,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(note);

        repository
            .Setup(x => x.IncrementViewCountAsync(
                note.Id,
                note.MaxViews,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var handler = new GetNoteQueryHandler(repository.Object);

        var query = new GetNoteQuery
        {
            Code = note.Code
        };

        var result = await handler.Handle(
            query,
            CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(note.Code, result.Code);
        Assert.Equal(note.Content, result.Content);
        Assert.False(result.RequiresPassword);

        repository.Verify(
            x => x.IncrementViewCountAsync(
                note.Id,
                note.MaxViews,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ReturnsNull_WhenNoteIsExpired()
    {
        var note = new Notes
        {
            Id = Guid.NewGuid(),
            Code = "expired123",
            Content = "Expired note",
            CreatedAt = DateTime.UtcNow.AddHours(-2),
            ExpiresAt = DateTime.UtcNow.AddMinutes(-1),
            MaxViews = 5,
            ViewCount = 0
        };

        var repository = new Mock<INoteRepository>();

        repository
            .Setup(x => x.GetByCodeAsync(
                note.Code,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(note);

        var handler = new GetNoteQueryHandler(repository.Object);

        var query = new GetNoteQuery
        {
            Code = note.Code
        };


        var result = await handler.Handle(
            query,
            CancellationToken.None);

        Assert.Null(result);

        repository.Verify(
            x => x.IncrementViewCountAsync(
                It.IsAny<Guid>(),
                It.IsAny<int?>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}