namespace ECommerce_Clean_Arch.Domain.Common.Interfaces;

public interface IAggregateRoot<out T> : IHasDomainEvents
    where T : struct, IAggregateRootId<T>
{
    T Id { get; }
    bool IsActive { get; }
    void Deactivate();
    void Activate();
}