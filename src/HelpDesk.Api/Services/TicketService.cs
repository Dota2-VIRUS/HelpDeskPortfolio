using HelpDesk.Api.Data;
using HelpDesk.Api.Domain;
using HelpDesk.Api.Dtos;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Api.Services;

public interface ITicketService
{
    Task<IReadOnlyList<TicketResponse>> GetAllAsync(TicketStatus? status, string? search, CancellationToken ct = default);
    Task<TicketResponse?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<TicketResponse> CreateAsync(CreateTicketDto dto, CancellationToken ct = default);
    Task<TicketResponse?> UpdateAsync(int id, UpdateTicketDto dto, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}

public class TicketService : ITicketService
{
    private readonly AppDbContext _db;

    public TicketService(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<TicketResponse>> GetAllAsync(TicketStatus? status, string? search, CancellationToken ct = default)
    {
        IQueryable<Ticket> query = _db.Tickets.AsNoTracking();

        if (status.HasValue)
            query = query.Where(t => t.Status == status.Value);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(t => t.Title.Contains(term) || t.Description.Contains(term));
        }

        var items = await query.OrderByDescending(t => t.CreatedAt).ToListAsync(ct);
        return items.Select(TicketResponse.From).ToList();
    }

    public async Task<TicketResponse?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var ticket = await _db.Tickets.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id, ct);
        return ticket is null ? null : TicketResponse.From(ticket);
    }

    public async Task<TicketResponse> CreateAsync(CreateTicketDto dto, CancellationToken ct = default)
    {
        var ticket = new Ticket
        {
            Title = dto.Title.Trim(),
            Description = dto.Description.Trim(),
            Priority = dto.Priority,
            Status = TicketStatus.Open,
            CreatedAt = DateTime.UtcNow
        };

        _db.Tickets.Add(ticket);
        await _db.SaveChangesAsync(ct);
        return TicketResponse.From(ticket);
    }

    public async Task<TicketResponse?> UpdateAsync(int id, UpdateTicketDto dto, CancellationToken ct = default)
    {
        var ticket = await _db.Tickets.FirstOrDefaultAsync(t => t.Id == id, ct);
        if (ticket is null) return null;

        ticket.Title = dto.Title.Trim();
        ticket.Description = dto.Description.Trim();
        ticket.Status = dto.Status;
        ticket.Priority = dto.Priority;
        ticket.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);
        return TicketResponse.From(ticket);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var ticket = await _db.Tickets.FirstOrDefaultAsync(t => t.Id == id, ct);
        if (ticket is null) return false;

        _db.Tickets.Remove(ticket);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}
