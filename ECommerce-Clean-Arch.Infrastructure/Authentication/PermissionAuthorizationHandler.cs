using ECommerce_Clean_Arch.Domain.Users.Constants;

using Microsoft.AspNetCore.Authorization;

namespace ECommerce_Clean_Arch.Infrastructure.Authentication;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement
    )
    {
        var permissions = context.User.Claims
            .Where(claim => claim.Type == Permissions.ClaimType)
            .Select(claim => claim.Value);
        if (permissions.Contains(requirement.Permission))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}