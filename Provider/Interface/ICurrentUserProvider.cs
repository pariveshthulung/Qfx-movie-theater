using QFX.Entity;

namespace QFX.Provider.Interface;

public interface ICurrentUserProvider
{
    bool IsLoggedIn();
    Task<User?> GetCurrentUser();
    long? GetCurrentUserId();
    string GetCurrentUserRole();


}
