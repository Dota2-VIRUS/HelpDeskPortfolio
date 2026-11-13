using System.ComponentModel.DataAnnotations;
using HelpDesk.Api.Domain;

namespace HelpDesk.Api.Dtos;

public class CreateTicketDto
{
    [Required, StringLength(200, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(2000)]
    public string Description { get; set; } = string.Empty;

    public TicketPriority Priority { get; set; } = TicketPriority.Medium;
}

public class UpdateTicketDto
{
    [Required, StringLength(200, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(2000)]
    public string Description { get; set; } = string.Empty;

    public TicketStatus Status { get; set; }
    public TicketPriority Priority { get; set; }
}

public record TicketResponse(
    int Id,
    string Title,
    string Description,
    TicketStatus Status,
    TicketPriority Priority,
    DateTime CreatedAt,
    DateTime? UpdatedAt)
{
    public static TicketResponse From(Ticket t) =>
        new(t.Id, t.Title, t.Description, t.Status, t.Priority, t.CreatedAt, t.UpdatedAt);
}
