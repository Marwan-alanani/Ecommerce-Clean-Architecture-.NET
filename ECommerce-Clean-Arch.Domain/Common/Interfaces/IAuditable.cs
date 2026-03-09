namespace ECommerce_Clean_Arch.Domain.Common.Interfaces;

public interface IAuditable
{
    public Guid? CreatedBy { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastModifiedAt { get; set; }
}