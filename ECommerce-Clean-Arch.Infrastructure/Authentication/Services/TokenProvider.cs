using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using ECommerce_Clean_Arch.Application.Authentication.Services;
using ECommerce_Clean_Arch.Application.Services;
using ECommerce_Clean_Arch.Domain.Common.Security;
using ECommerce_Clean_Arch.Domain.Roles;
using ECommerce_Clean_Arch.Domain.Users;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace ECommerce_Clean_Arch.Infrastructure.Authentication.Services;

public sealed class TokenProvider : ITokenProvider
{
    private readonly JwtConfig _jwtConfig;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IDateTimeProvider _dateTimeProvider;

    public TokenProvider(
        IOptions<JwtConfig> jwtConfig,
        IDateTimeProvider dateTimeProvider,
        UserManager<User> userManager,
        RoleManager<Role> roleManager
    )
    {
        _dateTimeProvider = dateTimeProvider;
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtConfig = jwtConfig.Value;
    }

    public async Task<string> GenerateAccessToken(User user)
    {
        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email!),
        ];

        var roleNames = await _userManager.GetRolesAsync(user);
        var userPermissions = new HashSet<string>();
        foreach (var roleName in roleNames)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) continue;
            var roleClaims = await _roleManager.GetClaimsAsync(role);
            var userClaims = await _userManager.GetClaimsAsync(user);

            userPermissions.UnionWith(
                roleClaims
                    .Where(claim => claim.Type == Permissions.ClaimType)
                    .Select(claim => claim.Value)
            );

            userPermissions.UnionWith(
                userClaims
                    .Where(claim => claim.Type == Permissions.ClaimType)
                    .Select(claim => claim.Value)
            );
        }


        claims.AddRange(
            userPermissions
                .Select(permission =>
                    new Claim(Permissions.ClaimType, permission)
                )
        );
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecretKey));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            _jwtConfig.Issuer,
            _jwtConfig.Audience,
            expires: _dateTimeProvider.UtcNow.AddMinutes(_jwtConfig.AccessTokenExpiryInMinutes),
            claims: claims,
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateOpaqueToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }

    public string HashOpaqueToken(string token)
    {
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
        return Convert.ToBase64String(hash);
    }
}