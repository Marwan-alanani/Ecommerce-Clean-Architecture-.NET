using Microsoft.AspNetCore.Authorization;

namespace ECommerce_Clean_Arch.Presentation.Attributes;

// make this and enum or some fixed type instead of string
public sealed class HasPermissionAttribute(string permission) : AuthorizeAttribute(policy: permission);