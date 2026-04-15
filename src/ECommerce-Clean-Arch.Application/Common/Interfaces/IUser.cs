namespace ECommerce_Clean_Arch.Application.Common.Interfaces;

public interface IUser
{
    public Guid? Id { get; }
    public string? Email { get; }
    public string? UserName { get; }
    public List<string>? Roles { get; }
    public List<string>? Permissions { get; }
}