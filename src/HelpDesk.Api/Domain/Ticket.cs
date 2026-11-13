namespace HelpDesk.Api.Domain;

public enum TicketStatus
{
    Open = 0,
    InProgress = 1,
    Resolved = 2
}

public enum TicketPriority
{
    Low = 0,
    Medium = 1,
    High = 2
}

public class Ticket
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TicketStatus Status { get; set; } = TicketStatus.Open;
    public TicketPriority Priority { get; set; } = TicketPriority.Medium;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
