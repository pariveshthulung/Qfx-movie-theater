using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QFX.Constants;
using QFX.data;
using QFX.ViewModels.ShowVm;

namespace QFX.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles ="Admin")]
public class ShowController: Controller
{
    private readonly ApplicationDbContext _context;
    private readonly INotyfService _notifyService;

    public ShowController(ApplicationDbContext context,INotyfService notyfService)
    {
        _context = context;
        _notifyService = notyfService;
    }
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult Upsert()
    {
        var vm = new UpsertVm();
        vm.Audis = _context.Audis.Include(x=>x.Location).ToList();
        vm.Movies = _context.Movies.ToList();
        
        return View(vm);
    }

}
