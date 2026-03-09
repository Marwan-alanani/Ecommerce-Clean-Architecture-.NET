using Microsoft.AspNetCore.Authorization;

namespace ECommerce_Clean_Arch.Presentation.Attributes;

public sealed class HasPermissionAttribute(string permission) : AuthorizeAttribute(policy: permission);