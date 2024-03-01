using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using QFX.data;
using QFX.Entity;
using QFX.Provider.Interface;
using QFX.Session;
using Microsoft.AspNetCore.Authorization;


namespace QFX.Areas.Public.Controllers;
[Area("Public")]
[AllowAnonymous]

public class LocationController : Controller
{
    private readonly ApplicationDbContext context;
    private readonly ICurrentUserProvider currentUserProvider;

    public LocationController(ApplicationDbContext context, ICurrentUserProvider currentUserProvider)
    {
        this.context = context;
        this.currentUserProvider = currentUserProvider;

    }

    [HttpPost]
    public async Task<IActionResult> Select(long LocationId)
    {
        var userId = currentUserProvider.GetCurrentUserId();
        // add to session
        // if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString(SessionVariable.sessionKeyLocationId)))
        // {
        //     Console.WriteLine("empty");
        //     HttpContext.Session.SetString(SessionVariable.sessionKeyLocationId, LocationId.ToString());
        // }
        // if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("sessionKeyLocationId")))
        // {
        //     Console.WriteLine("empty");
        // }
        HttpContext.Session.SetString("sessionKeyLocationId", LocationId.ToString());
        if (currentUserProvider.IsLoggedIn())
        {
            using (var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var existingPreference = await context.UserLocationPreferences.Where(x => x.UserId == userId).FirstOrDefaultAsync();
                if (existingPreference == null)
                {
                    // Not created
                    var preference = new UserLocationPreference();
                    preference.UserId = userId.Value;
                    preference.LocationId = LocationId;
                    context.UserLocationPreferences.Add(preference);
                    await context.SaveChangesAsync();
                }
                else
                {
                    existingPreference.LocationId = LocationId;
                    context.Update(existingPreference);
                }
                tx.Complete();
            }

        }
        return Redirect("/Public/Public/Index");
        // return RedirectToAction(this.Request.Headers.Referer)
    }
}
