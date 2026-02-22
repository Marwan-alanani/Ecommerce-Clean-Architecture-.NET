
using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Authentication.Common;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Authentication.Queries;

public class LoginQueryHandler : IQueryHandler<LoginQuery, AuthenticationResult>
{
    public async Task<Result<AuthenticationResult>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}