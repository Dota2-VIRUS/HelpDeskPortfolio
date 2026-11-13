using HelpDesk.Api.Domain;
using HelpDesk.Api.Dtos;
using HelpDesk.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _service;

    public TicketsController(ITicketService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TicketResponse>>> GetAll(
        [FromQuery] TicketStatus? status, [FromQuery] string? search, CancellationToken ct)
        => Ok(await _service.GetAllAsync(status, search, ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TicketResponse>> GetById(int id, CancellationToken ct)
    {
        var ticket = await _service.GetByIdAsync(id, ct);
        return ticket is null ? NotFound() : Ok(ticket);
    }

    [HttpPost]
    public async Task<ActionResult<TicketResponse>> Create(CreateTicketDto dto, CancellationToken ct)
    {
        var created = await _service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<TicketResponse>> Update(int id, UpdateTicketDto dto, CancellationToken ct)
    {
        var updated = await _service.UpdateAsync(id, dto, ct);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
        => await _service.DeleteAsync(id, ct) ? NoContent() : NotFound();
}
