using AutoMapper;
using ECommerce_Clean_Arch.Domain.Users;

namespace ECommerce_Clean_Arch.Application.Authentication.Common;

public record AuthenticationResult
{
    private AuthenticationResult()
    {
    }

    public AuthenticationResult(
        Guid id,
        string username,
        string email,
        string firstName,
        string lastName,
        string token
    )
    {
        Id = id;
        UserName = username;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Token = token;
    }

    public Guid Id { get; init; }
    public string UserName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string Token { get; init; } = null!; // Jwt access token

    private class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<User, AuthenticationResult>();
            CreateMap<(User user, string token), AuthenticationResult>()
                .ForMember(d => d.Token, opt => opt.MapFrom(s => s.token))
                .IncludeMembers(src => src.user);
        }
    }
}