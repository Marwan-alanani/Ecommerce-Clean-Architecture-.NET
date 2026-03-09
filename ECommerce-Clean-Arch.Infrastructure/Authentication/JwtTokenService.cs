using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using ECommerce_Clean_Arch.Application.Authentication.Services;
using ECommerce_Clean_Arch.Application.Services;
using ECommerce_Clean_Arch.Domain.Users;
using ECommerce_Clean_Arch.Domain.Users.Constants;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace ECommerce_Clean_Arch.Infrastructure.Authentication;

public sealed class JwtTokenService : IJwtTokenService
{
    private readonly JwtConfig _jwtConfig;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    private readonly IDateTimeProvider _dateTimeProvider;

    public JwtTokenService(
        IOptions<JwtConfig> jwtConfig,
        IDateTimeProvider dateTimeProvider,
        UserManager<User> userManager,
        RoleManager<IdentityRole<Guid>> roleManager
    )
    {
        _dateTimeProvider = dateTimeProvider;
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtConfig = jwtConfig.Value;
    }

    public async Task<string> Generate(User user)
    {
        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email!),
        ];

        var roleNames = await _userManager.GetRolesAsync(user);
        var userPolicies = new HashSet<string>();
        foreach (var roleName in roleNames)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) continue;
            var roleClaims = await _roleManager.GetClaimsAsync(role);
            var userClaims = await _userManager.GetClaimsAsync(user);

            userPolicies.UnionWith(
                roleClaims
                    .Where(claim => claim.Type == Permissions.ClaimType)
                    .Select(claim => claim.Value)
            );

            userPolicies.UnionWith(
                userClaims
                    .Where(claim => claim.Type == Permissions.ClaimType)
                    .Select(claim => claim.Value)
            );
        }


        claims.AddRange(
            userPolicies
                .Select(policy =>
                    new Claim(Permissions.ClaimType, policy)
                )
        );
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecretKey));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            _jwtConfig.Issuer,
            _jwtConfig.Audience,
            expires: _dateTimeProvider.UtcNow.AddMinutes(_jwtConfig.ExpiryInMinutes),
            claims: claims,
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}