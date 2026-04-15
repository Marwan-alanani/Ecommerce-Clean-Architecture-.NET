namespace ECommerce_Clean_Arch.Domain.Common.Security;

public interface IPermissions
{
    IEnumerable<string> All();
}