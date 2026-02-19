namespace ECommerce_Clean_Arch.Domain.Common;

public interface IAuditable
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}