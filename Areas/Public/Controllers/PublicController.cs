using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QFX.data;

namespace QFX.Areas.Public.Controllers;
[Area("Public")]
[AllowAnonymous]
public class PublicController : Controller
{
    private readonly ApplicationDbContext _context;
    public PublicController(ApplicationDbContext context)
    {
        _context = context;
    }
    public IActionResult Index()
    {
        return View();
    }
}