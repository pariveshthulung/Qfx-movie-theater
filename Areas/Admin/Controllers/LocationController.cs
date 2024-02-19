using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QFX.data;
using QFX.Models;
using QFX.ViewModels.LocationVm;

namespace QFX.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles ="Admin")]
public class LocationController : Controller
{
    private readonly INotyfService _notifyService;
    private readonly ApplicationDbContext _context;
    public LocationController(ApplicationDbContext context, INotyfService notyfService)
    {
        _notifyService = notyfService;
        _context = context;
    }
    public IActionResult Index()
    {
        var vm = new IndexVm();
        vm.Locations = _context.Locations.ToList();
        return View(vm);
    }  
    public async Task<IActionResult> Upsert(long? ID)
    {
        var vm = new UpsertVm();
        if (ID != null)
        {
            var location = await _context.Locations.FirstOrDefaultAsync(x => x.ID == ID);
            vm.CityName = location.CityName;
        }
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Upsert(long? ID,UpsertVm vm)
    {
        try
        {
            if (ID != null)
            {
                var location = await _context.Locations.FirstOrDefaultAsync(x => x.ID == ID);
                location.CityName = vm.CityName;
            }
            else
            {
                var location = new Models.Location();
                location.CityName = vm.CityName;
                _context.Locations.Add(location);
            }
            _notifyService.Success("Done successfully!!");
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _notifyService.Error("Operation fails" + e.Message);
            return View(vm);
        }
    }
    public async Task<IActionResult> Delete(long ID)
    {
        try
        {
            _context.Locations.Remove(_context.Locations.FirstOrDefault(x => x.ID == ID));
            await _context.SaveChangesAsync();
            _notifyService.Success("Location Deleted!!!");
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _notifyService.Error("Operation fails" + e.Message);
            return RedirectToAction("Index");
        }
    }



}
