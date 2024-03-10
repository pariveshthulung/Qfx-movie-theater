using Microsoft.EntityFrameworkCore;
using QFX.data;
using QFX.Provider.Interface;


namespace QFX.Provider;

public class CurrentLocationProvider : ICurrentLocationProvider
{
   
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ApplicationDbContext _context;
    private readonly ICurrentUserProvider _currentUser;

    public CurrentLocationProvider(ApplicationDbContext context,IHttpContextAccessor contextAccessor,ICurrentUserProvider currentUser)
    {
        _context = context;
        _currentUser = currentUser;
        _contextAccessor = contextAccessor;

    }

    public long GetCurrentLocationIDAsync()
    {
        var currentUserId = _currentUser?.GetCurrentUserId();

        // get LocationID from session
        if (_contextAccessor.HttpContext?.Session.GetString("sessionKeyLocationId") != null)
        {
            var sessionLocationID = _contextAccessor.HttpContext?.Session.GetString("sessionKeyLocationId").ToString();
            return Convert.ToInt64(sessionLocationID);

        }
        else if (currentUserId != null)
        {
            return _context.UserLocationPreferences.Where(x => x.UserId == currentUserId).Select(x => x.LocationId).FirstOrDefault();
        }
        else
        {
            return _context.Locations.Select(x => x.ID).FirstOrDefault();
        }
    }
}
