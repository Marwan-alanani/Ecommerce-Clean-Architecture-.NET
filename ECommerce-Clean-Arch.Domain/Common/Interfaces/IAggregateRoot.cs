namespace ECommerce_Clean_Arch.Domain.Common.Interfaces;

internal interface IAggregateRoot<T> : IHasDomainEvents
    where T : struct, IEquatable<T>
{
    bool IsActive { get; }
    void Deactivate();
    void Activate();
}