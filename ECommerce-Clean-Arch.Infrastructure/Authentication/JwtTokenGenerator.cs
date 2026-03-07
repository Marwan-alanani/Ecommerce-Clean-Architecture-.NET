using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using ECommerce_Clean_Arch.Application.Authentication.Interfaces;
using ECommerce_Clean_Arch.Application.Services;
using ECommerce_Clean_Arch.Domain.Users;
using ECommerce_Clean_Arch.Domain.Users.Constants;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace ECommerce_Clean_Arch.Infrastructure.Authentication;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtConfig _jwtConfig;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    private readonly IDateTimeProvider _dateTimeProvider;

    public JwtTokenGenerator(
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
        var roleNames = await _userManager.GetRolesAsync(user);
        var rolePermissions = new HashSet<string>();
        foreach (var roleName in roleNames)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) continue;
            var roleClaims = await _roleManager.GetClaimsAsync(role);

            rolePermissions.UnionWith(
                roleClaims
                    .Where(claim => claim.Type == RolePermissions.ClaimType)
                    .Select(claim => claim.Value)
            );
        }


        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        ];
        claims.AddRange(
            rolePermissions
                .Select(permission =>
                    new Claim(RolePermissions.ClaimType, permission)
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