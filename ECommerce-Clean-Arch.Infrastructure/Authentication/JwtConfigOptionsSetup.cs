using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace ECommerce_Clean_Arch.Infrastructure.Authentication;

internal sealed class JwtConfigOptionsSetup : IConfigureOptions<JwtConfig>
{
    private readonly IConfiguration _configuration;

    public JwtConfigOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(JwtConfig options)
    {
        _configuration.Bind(JwtConfig.SectionName, options);
    }
}