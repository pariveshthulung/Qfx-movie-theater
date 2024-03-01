using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QFX.data;
using QFX.Models;
using QFX.ViewModels.AudiVm;

namespace QFX.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin,Employee")]
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
            vm.Name = audi.Name;
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
                audi.Name = vm.Name;
                audi.Row = vm.Row;
                audi.Column = vm.Column;
                audi.LocationID = vm.LocationID;
                _context.Seats.RemoveRange(await _context.Seats.Where(x => x.AudiID == ID).ToListAsync());
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
                var existAudi = _context.Audis.Where(x=>x.LocationID==vm.LocationID).ToList();
                int n = existAudi.Count;
                n++;
                var audi = new Audi();
                audi.Name = "Audi"+ n;
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
            return Content(e.Message);
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
