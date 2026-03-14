using System.Security.Claims;

using ECommerce_Clean_Arch.Application.Common.Interfaces;

using Perms = ECommerce_Clean_Arch.Domain.Common.Security.Permissions;

namespace ECommerce_Clean_Arch.Presentation.Services;

public class CurrentUser : IUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }


    public Guid? Id
    {
        get
        {
            var idString = _httpContextAccessor.HttpContext?.User.FindFirstValue
                (ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(idString, out _)) return null;
            return Guid.Parse(idString);
        }
    }

    public string? Email
    {
        get
        {
            var email = _httpContextAccessor.HttpContext?.User.FindFirstValue
                (ClaimTypes.Email);
            return email;
        }
    }

    public List<string>? Roles => _httpContextAccessor.HttpContext?.User.FindAll(ClaimTypes.Role)
        .Select(x => x.Value)
        .ToList();

    public List<string>? Permissions
    {
        get
        {
            return _httpContextAccessor.HttpContext?.User.Claims
                .Where(claim => claim.Type == Perms.ClaimType)
                .Select(claim => claim.Value)
                .ToList();
        }
    }
}