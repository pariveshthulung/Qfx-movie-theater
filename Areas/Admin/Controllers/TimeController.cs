using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QFX.data;
using QFX.Models;
using QFX.Provider.Interface;
using QFX.ViewModels.ShowTimeVM;

namespace QFX.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles ="Admin,Employee")]
public class TimeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly INotyfService _notifyService;
    private readonly ICurrentUserProvider _currentUser;

    public TimeController(ApplicationDbContext context,INotyfService notyfService, ICurrentUserProvider currentUser)
    {
        _context = context;
        _notifyService = notyfService;
        _currentUser = currentUser;
    }

     public IActionResult Index(long ShowDateID)
    {
        var vm = new TimeIndexVm();
        vm.ShowDate = _context.ShowDates.Where(x=>x.ID==ShowDateID).Include(x=>x.Show).ThenInclude(x=>x.Audi).ThenInclude(y=>y.Location).Include(x=>x.Show).ThenInclude(x=>x.Movie).FirstOrDefault();
        vm.ShowTime = _context.ShowTimes.Where(x=>x.ShowDateID==ShowDateID).ToList();
        return View(vm);
    }
    public IActionResult Upsert(long? ShowTimeID, long ShowDateID)
    {
        var vm = new TimeUpsertVm();
        if (ShowTimeID != null)
        {
            var ShowTime = _context.ShowTimes.FirstOrDefault(x => x.ID == ShowTimeID);
            vm.Time = ShowTime.Time;
        }
        vm.ShowDateID = ShowDateID;
        return View(vm);
    }
    [HttpPost]
    public async Task<IActionResult> Upsert(long? ShowTimeID, TimeUpsertVm vm)
    {
        long Id = 0;
        if (ShowTimeID != null)
        {
            var ShowTime = _context.ShowTimes.FirstOrDefault(x => x.ID == ShowTimeID);
            ShowTime.Time = vm.Time;
            Id = ShowTime.ShowDateID;
            _notifyService.Success("Time Updated!!!");

            await _context.SaveChangesAsync();
        }
        else
        {
            var showTime = new ShowTime();
            showTime.Time = vm.Time;
            showTime.ShowDateID = vm.ShowDateID;
            Id = vm.ShowDateID;
            _context.ShowTimes.Add(showTime);
            _notifyService.Success("Time Added!!!");

            // 
            //  foreach (var id in seatId)
            //     {
            //         var showSeat = new ShowSeat();
            //         showSeat.ShowID = show.ID;
            //         showSeat.SeatID = id;
            //         showSeat.ShowSeatStatus = SeatStatusConstants.Active;
            //         _context.ShowSeats.Add(showSeat);
            //         await _context.SaveChangesAsync();
            //     }

            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index", new { ShowDateID = Id });
    }
    [HttpPost]
    public async Task<IActionResult> Delete(long ShowTimeID)
    {
        var showTime = _context.ShowTimes.FirstOrDefault(x => x.ID == ShowTimeID);
        _context.ShowTimes.Remove(showTime);
        await _context.SaveChangesAsync();
        _notifyService.Success("Time deleted!!!");
        return RedirectToAction("Index", new { ShowDateID = showTime.ShowDateID });
    }

}
