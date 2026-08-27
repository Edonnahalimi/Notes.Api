using MediatR;
using Microsoft.AspNetCore.Mvc;
using TemporaryNotes.Application.DTOs;
using TemporaryNotes.Application.Note.Commands;
using TemporaryNotes.Application.Note.Queries;

namespace TemporaryNotes.Api.Controllers;

[ApiController]
[Route("api/notes")]
public class NotesController : ControllerBase
{
    private readonly IMediator _mediator;

    public NotesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateNoteCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{code}")]
    public async Task<IActionResult> GetByCode([FromRoute] string code,  CancellationToken cancellationToken)
    {
        var query = new GetNoteQuery { Code = code };

        var result = await _mediator.Send(query, cancellationToken);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost("{code}/unlock")]
    public async Task<IActionResult> Unlock([FromRoute] string code, [FromBody] UnlockNoteRequest request, CancellationToken cancellationToken)
    {
        var command = new UnlockNoteCommand { Code = code, Password = request.Password };

        var result = await _mediator.Send(command, cancellationToken);

        if (result is null)
            return Unauthorized();

        return Ok(result);
    }

    [HttpPut("{code}")]
    public async Task<IActionResult> Update([FromRoute] string code, [FromBody] UpdateNoteRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateNoteCommand
        {
            Code = code,
            Content = request.Content,
            ExpiresInMinutes = request.ExpiresInMinutes,
            MaxViews = request.MaxViews,
            Password = request.Password,
            NewPassword = request.NewPassword
        };

        var result = await _mediator.Send(
            command,
            cancellationToken);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpDelete("{code}")]
    public async Task<IActionResult> Delete(string code, [FromQuery] string? password, CancellationToken cancellationToken)
    {
        var command = new DeleteNoteCommand { Code = code, Password = password };

        var deleted = await _mediator.Send(command, cancellationToken);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}