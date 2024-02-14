using System.Security.Claims;
using QFX.data;
using QFX.Entity;
using QFX.Provider.Interface;

namespace QFX;

public class CurrentUserProvider : ICurrentUserProvider
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CurrentUserProvider(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<User?> GetCurrentUser()
    {
        var currentUserId = GetCurrentUserId();
        if(!currentUserId.HasValue) return null;

        return await _context.Users.FindAsync(currentUserId.Value);
        
    }
    public long? GetCurrentUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue("ID");
        if(string.IsNullOrWhiteSpace(userId)){
            return null;
        }
        return Convert.ToInt64(userId);
    }

    public bool IsLoggedIn()
        => GetCurrentUserId() != null;
}
