using Microsoft.AspNetCore.Mvc;
using QFX.data;

namespace QFX.Areas.Admin.Controllers;

public class DashboardController : Controller
{
    private readonly ApplicationDbContext _context;
    public DashboardController(ApplicationDbContext context)
    {
        _context = context;
    }
    public IActionResult Index()
    {
        return View();
    }
}
