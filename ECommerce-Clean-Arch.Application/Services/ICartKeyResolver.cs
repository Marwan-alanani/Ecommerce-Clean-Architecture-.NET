namespace ECommerce_Clean_Arch.Application.Services;

public interface ICartKeyResolver
{
    /// <summary>
    /// Gets appropriate key for cart ... if logged-in session returns a different key from when guest
    ///  session
    /// </summary>
    /// <returns>Appropriate cart key</returns>
    public string GetCartKey();

    public string GetUserKey(Guid userId);

    public string GetGuestKey(string guestId);
}