namespace ECommerce_Clean_Arch.Domain.Users;

public class User
{
    private User()
    {
    }

    private User(
        Guid id,
        string username,
        string firstName,
        string lastName,
        string email,
        string password,
        DateTime createdAt,
        DateTime updatedAt
    )
    {
        Id = id;
        Username = username;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public Guid Id { get; private set; }
    public string Username { get; private set; } = null!;
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Password { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public static User Create(
        string username,
        string firstName,
        string lastName,
        string email,
        string password
    )
    {
        return new User(
            Guid.NewGuid(),
            username,
            firstName,
            lastName,
            email,
            password,
            DateTime.UtcNow,
            DateTime.UtcNow
        );
    }
}