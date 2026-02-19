using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ECommerce_Clean_Arch.Application.Authentication;
using ECommerce_Clean_Arch.Domain.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace ECommerce_Clean_Arch.Infrastructure.Authentication;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtConfig _jwtConfig;

    public JwtTokenGenerator(IOptions<JwtConfig> jwtConfig)
    {
        _jwtConfig = jwtConfig.Value;
    }

    public string Generate(User user)
    {
        // TODO : Add user role in the jwtToken
        List<Claim> claims =
        [
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        ];
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecretKey));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _jwtConfig.Issuer,
            audience: _jwtConfig.Audience,
            expires: DateTime.UtcNow.AddMinutes(_jwtConfig.ExpiryInMinutes),
            claims: claims,
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}