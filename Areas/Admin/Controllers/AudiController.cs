using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QFX.data;
using QFX.Models;
using QFX.ViewModels.AudiVm;

namespace QFX.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class AudiController : Controller
{
    private readonly INotyfService _notifyService;
    private readonly ApplicationDbContext _context;
    public AudiController(ApplicationDbContext context, INotyfService notyfService)
    {
        _notifyService = notyfService;
        _context = context;
    }
    public IActionResult Index()
    {
        var vm = new IndexVm();
        vm.Audis = _context.Audis.Include(x => x.Location).ToList();
        return View(vm);
    }

    public async Task<IActionResult> Upsert(long? ID)
    {
        var vm = new UpsertVm();
        if (ID != null)
        {
            var audi = await _context.Audis.FirstOrDefaultAsync(x => x.ID == ID);
            vm.Row = audi.Row;
            vm.Column = audi.Column;
            vm.LocationID = audi.LocationID;
        }
        vm.Locations = await _context.Locations.ToListAsync();
        return View(vm);
    }
    [HttpPost]
    public async Task<IActionResult> Upsert(long? ID, UpsertVm vm)
    {
        try
        {
            if (ID != null)
            {
                var audi = await _context.Audis.FirstOrDefaultAsync(x => x.ID == ID);
                audi.Row = vm.Row;
                audi.Column = vm.Column;
                audi.LocationID = vm.LocationID;
                _context.Seats.RemoveRange(_context.Seats.Where(x => x.AudiID == ID).ToList());
                await _context.SaveChangesAsync();

                char x = 'A';
                for (var i = 1; i <= vm.Column; i++)
                {
                    for (var j = 1; j <= vm.Row; j++)
                    {
                        var seat = new Seat();
                        seat.SeatName = x.ToString() + j;
                        seat.AudiID = audi.ID;
                        _context.Seats.Add(seat);
                        await _context.SaveChangesAsync();
                    }
                    x++;
                }
            }
            else
            {
                var audi = new Audi();
                audi.Row = vm.Row;
                audi.Column = vm.Column;
                audi.LocationID = vm.LocationID;
                _context.Audis.Add(audi);
                await _context.SaveChangesAsync();

                char x = 'A';
                for (var i = 1; i <= vm.Column; i++)
                {
                    for (var j = 1; j <= vm.Row; j++)
                    {
                        var seat = new Seat();
                        seat.SeatName = x.ToString() + j;
                        seat.AudiID = audi.ID;
                        _context.Seats.Add(seat);
                        await _context.SaveChangesAsync();
                    }
                    x++;
                }
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
            _context.Audis.Remove(_context.Audis.FirstOrDefault(x => x.ID == ID));
            _context.Seats.RemoveRange(_context.Seats.Where(x => x.AudiID == ID).ToList());
            await _context.SaveChangesAsync();
            _notifyService.Success("Language Deleted!!!");
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _notifyService.Error("Operation fails" + e.Message);
            return RedirectToAction("Index");
        }
    }

}
