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
        if (!currentUserId.HasValue) return null;

        return await _context.Users.FindAsync(currentUserId.Value);

    }
    public long? GetCurrentUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue("ID");
        if (string.IsNullOrWhiteSpace(userId))
        {
            return null;
        }
        return Convert.ToInt64(userId);
    }

    public string? GetCurrentUserName()
    {
        var userId = GetCurrentUserId();
        if (userId != null)
        {
            var userName = _context.Users.Where(x => x.ID == userId).Select(x => x.Name).FirstOrDefault();
            return userName;
        }
        else
        {
            return null;
        }
    }

    public string? GetCurrentUserRole()
    {
        // var user = _context
        var userRole = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role);
        if (string.IsNullOrWhiteSpace(userRole))
        {
            return null;
        }
        return userRole;
    }

    public bool IsLoggedIn()
        => GetCurrentUserId() != null;
}
