using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using QFX.Constants;
using QFX.data;
using QFX.Entity;
using QFX.Models;

namespace QFX.Areas.Admin.Controllers;
[Area("Admin")]
[AllowAnonymous]
public class SeederController : Controller
{
    private readonly ApplicationDbContext _context;
    public SeederController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> SeedAdmin()
    {
        try
        {

            var AdminExist = await _context.Users.AnyAsync(x => x.UserType == UserTypeConstants.Admin);
            if (!AdminExist)
            {
                using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                var admin = new User
                {
                    Name = "Admin",
                    Email = "QfxAdmin@gmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin"),
                    UserType = UserTypeConstants.Admin
                };
                await _context.AddAsync(admin);
                await _context.SaveChangesAsync();
                tx.Complete();
                return Content("admin added");
            }
            return Content("Admin exist");
        }
        catch (Exception e)
        {
            return Content(e.Message);
        }
    }

    public async Task<IActionResult> SeedGenre()
    {
        try
        {
            var GenreExist = await _context.Locations.AnyAsync();
            if (!GenreExist)
            {
                using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                List<Models.Genre> genres = new List<Models.Genre>()
                {
                    new() {Name ="Action"},
                    new() {Name ="Thriller"},
                    new() {Name ="Drama"},
                    new() {Name ="Horror"},
                    new() {Name ="Drama"},
                    new() {Name ="Science fiction"},
                    new() {Name ="Crime film"},
                    new() {Name ="Western"},
                    new() {Name ="Animation"},
                    new() {Name ="Fantasy"},
                    new() {Name ="Television"},
                    new() {Name ="Romance"},
                    new() {Name ="Comedy"},
                    new() {Name ="Historical Fiction"},
                    new() {Name ="Romance"},
                    new() {Name ="Comedy"},
                    new() {Name ="Musical genre"},
                    new() {Name ="Comedic genres"},
                    new() {Name ="Experimental"},
                    new() {Name ="Documentary"},
                    new() {Name ="Musical"},
                    new() {Name ="Narrative"},
                    new() {Name ="Crime"},
                    new() {Name ="Fiction"},
                    new() {Name ="Science fiction"},
                    new() {Name ="Fantasy"},
                    new() {Name ="Mystery"},
                    new() {Name ="Action fiction"},
                    new() {Name ="Action/Adventure"},
                    new() {Name ="Adventure"},
                    new() {Name ="Satire"},
                    new() {Name ="History"},
                    new() {Name ="Noir"},
                    new() {Name ="Narrative"},
                    new() {Name ="Thriller"},
                    new() {Name ="Short"},
                    new() {Name ="Hindi cinema"},
                    new() {Name ="Mystery"},
                    new() {Name ="Romantic comedy"},
                    new() {Name ="Historical drama"},
                    new() {Name ="Animated film"},
                    new() {Name ="Historical film"},
                    new() {Name ="Suspense"},
                    new() {Name ="War"},
                    new() {Name ="Melodrama"},
                    new() {Name ="Music"},
                    new() {Name ="Exploitation"},
                    new() {Name ="Science"},
                    new() {Name ="Epic"},
                    new() {Name ="Disaster"},
                    new() {Name ="Art"}
                };
                await _context.Genres.AddRangeAsync(genres);
                await _context.SaveChangesAsync();
                tx.Complete();
                return Content("Added Genre!!!");
            }
            return Content("Genre exist!!!");
        }
        catch(Exception e)
        {
            return Content(e.Message);
        }
    }
    public async Task<IActionResult> SeedLanguage()
    {
        try
        {
            var LanguageExist = await _context.Locations.AnyAsync();
            if (!LanguageExist)
            {
                using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                List<Models.Language> languages = new List<Models.Language>()
                {
                    new() {Name ="English"},
                    new() {Name ="Nepali"},
                    new() {Name ="Hindi"},
                    new() {Name ="Japanese"},
                    
                };
                await _context.Languages.AddRangeAsync(languages);
                await _context.SaveChangesAsync();
                tx.Complete();
                return Content("Added Language!!!");
            }
            return Content("language exist!!!");
        }
        catch(Exception e)
        {
            return Content(e.Message);
        }
    }
}


