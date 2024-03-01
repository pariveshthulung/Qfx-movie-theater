using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using QFX.data;
using QFX.Migrations;
using QFX.Models;
using QFX.Provider.Interface;
using QFX.ViewModels.DateVm;

namespace QFX.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin,Employee")]
public class DateController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly INotyfService _notifyService;
    private readonly ICurrentUserProvider _currentUser;

    public DateController(ApplicationDbContext context, INotyfService notyfService, ICurrentUserProvider currentUser)
    {
        _context = context;
        _notifyService = notyfService;
        _currentUser = currentUser;
    }

    public IActionResult Index(long ShowID)
    {
        var vm = new DateIndexVm();
        vm.Show = _context.Shows.Where(x => x.ID == ShowID).Include(x => x.Movie).Include(x => x.Audi).ThenInclude(x => x.Location).FirstOrDefault();
        vm.ShowDate = _context.ShowDates.Where(x => x.ShowID == ShowID).Include(x => x.Show).ThenInclude(x => x.Movie).Include(x => x.Show).ThenInclude(x => x.Audi).ToList();
        return View(vm);
    }
    public IActionResult Upsert(long? ShowDateID, long ShowID)
    {
        var vm = new DateUpsertVm();
        if (ShowDateID != null)
        {
            var ShowDate = _context.ShowDates.FirstOrDefault(x => x.ID == ShowDateID);
            vm.Date = ShowDate.Date;
        }
        vm.ShowID = ShowID;
        return View(vm);
    }
    [HttpPost]
    public async Task<IActionResult> Upsert(long? ShowDateID, DateUpsertVm vm)
    {
        long Id = 0;
        if (ShowDateID != null)
        {
            var ShowDate = _context.ShowDates.FirstOrDefault(x => x.ID == ShowDateID);
            ShowDate.Date = vm.Date;
            Id = ShowDate.ShowID;
            _notifyService.Success("date Updated!!!");

            await _context.SaveChangesAsync();
        }
        else
        {
            var showDate = new ShowDate();
            showDate.Date = vm.Date;
            showDate.ShowID = vm.ShowID;
            Id = vm.ShowID;
            _context.ShowDates.Add(showDate);
            _notifyService.Success("date Added!!!");

            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index", new { ShowID = Id });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(long ShowDateID)
    {
        var showDate = _context.ShowDates.FirstOrDefault(x => x.ID == ShowDateID);
        _context.ShowDates.Remove(showDate);
        await _context.SaveChangesAsync();
        _notifyService.Success("date deleted!!!");
        return RedirectToAction("Index", new { ShowID = showDate.ShowID });
    }
}
