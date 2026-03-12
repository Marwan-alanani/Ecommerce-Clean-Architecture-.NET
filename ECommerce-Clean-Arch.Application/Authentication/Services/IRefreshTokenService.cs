using ECommerce_Clean_Arch.Domain.RefreshTokens;
using ECommerce_Clean_Arch.Domain.Users;

using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Authentication.Services;

public interface IRefreshTokenService
{
    /// <summary>
    /// given a refresh token object and its unhashed value hashes the refresh token and adds it to db
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <param name="opaqueValue"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>RefreshToken</returns>
    Task CreateRefreshToken(
        RefreshToken refreshToken,
        string opaqueValue,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// hashes token -> checks db for user id by hash -> gets user by user id ...
    /// if any step fails returns an error object
    /// </summary>
    /// <returns> Task of result of user </returns>
    Task<Result<User>> GetUserAsync(string token);

    /// <summary>
    /// Check if token is valid
    /// </summary>
    /// <param name="token"></param>
    /// <param name="ipAddress"></param>
    /// <param name="userAgent"></param>
    /// <returns></returns>
    Task<bool> IsTokenValidAsync(
        string token,
        string? ipAddress,
        string userAgent
    );
}