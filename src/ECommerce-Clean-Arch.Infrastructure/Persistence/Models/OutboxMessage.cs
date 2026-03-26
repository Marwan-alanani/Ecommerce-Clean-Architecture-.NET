namespace ECommerce_Clean_Arch.Infrastructure.Persistence.Models;

public class OutboxMessage
{
    public OutboxMessage(
        string type,
        string content,
        DateTime occuredOn,
        Guid aggregateId
    )
    {
        Id = Guid.NewGuid();
        Type = type;
        Content = content;
        OccuredOn = occuredOn;
        AggregateId = aggregateId;
    }

    public Guid Id { get; set; }
    public Guid AggregateId { get; set; }
    public long AggregateVersion { get; set; }
    public string Type { get; set; } // Type in code
    public string Content { get; set; } // Json content
    public DateTime OccuredOn { get; set; }
    public string? Error { get; set; }
    public DateTime? ProcessedOn { get; set; }
}