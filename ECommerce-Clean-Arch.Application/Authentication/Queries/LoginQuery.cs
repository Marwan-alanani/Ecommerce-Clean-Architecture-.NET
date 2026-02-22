using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Authentication.Common;

namespace ECommerce_Clean_Arch.Application.Authentication.Queries;

public record LoginQuery(
    string Email,
    string Password
) : IQuery<AuthenticationResult>;