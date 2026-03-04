namespace ECommerce_Clean_Arch.Infrastructure.Persistence.Models;

public class OutboxMessage
{
    private OutboxMessage() // ef core needs that
    {
    }

    public OutboxMessage(
        string type,
        string content,
        DateTime occuredOn
    )
    {
        Id = Guid.NewGuid();
        Type = type;
        Content = content;
        OccuredOn = occuredOn;
    }

    public Guid Id { get; set; }
    public string Type { get; set; } // Type in code
    public string Content { get; set; } // Json content
    public DateTime OccuredOn { get; set; }
    public DateTime? ProcessedOn { get; set; }
}