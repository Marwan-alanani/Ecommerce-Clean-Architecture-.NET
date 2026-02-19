using FluentResults;

namespace ECommerce_Clean_Arch.Application.Users.Errors;

public class UserEmailFoundError(string email) :
    Error($"User with the email '{email}' was " + $"found! ");