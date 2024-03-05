using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QFX.Constants;
using QFX.data;
using QFX.Models;
using QFX.Provider.Interface;
using QFX.ViewModels.ShowTimeVM;

namespace QFX.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin,Employee")]
public class TimeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly INotyfService _notifyService;
    private readonly ICurrentUserProvider _currentUser;

    public TimeController(ApplicationDbContext context, INotyfService notyfService, ICurrentUserProvider currentUser)
    {
        _context = context;
        _notifyService = notyfService;
        _currentUser = currentUser;
    }

    public IActionResult Index(long ShowDateID)
    {
        var vm = new TimeIndexVm();
        vm.ShowDate = _context.ShowDates.Where(x => x.ID == ShowDateID).Include(x => x.Show).ThenInclude(x => x.Audi).ThenInclude(y => y.Location).Include(x => x.Show).ThenInclude(x => x.Movie).FirstOrDefault();
        vm.ShowTime = _context.ShowTimes.Where(x => x.ShowDateID == ShowDateID).ToList();
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
            Id = vm.ShowDateID;
            var DoTimeExist = await _context.ShowTimes.AnyAsync(x => x.Time == vm.Time && x.ShowDateID==vm.ShowDateID);
            if (!DoTimeExist)
            {
                var showTime = new ShowTime();
                showTime.Time = vm.Time;
                showTime.ShowDateID = vm.ShowDateID;
                _context.ShowTimes.Add(showTime);
                await _context.SaveChangesAsync();


                // var ShowTimeDB = _context.ShowTimes.Where(x=>x.ID==ShowTimeID).Include(x => x.ShowDate).ThenInclude(y => y.Show).FirstOrDefault();
                var ShowDateDB = _context.ShowDates.Where(x => x.ID == vm.ShowDateID).Include(x => x.Show).FirstOrDefault();

                var seatId = _context.Seats.Where(x => x.AudiID == ShowDateDB.Show.AudiID).Select(X => X.ID).ToList();

                foreach (var id in seatId)
                {
                    var showSeat = new ShowSeat();
                    showSeat.ShowTimeID = showTime.ID;
                    showSeat.SeatID = id;
                    showSeat.ShowSeatStatus = SeatStatusConstants.Active;
                    _context.ShowSeats.Add(showSeat);
                    await _context.SaveChangesAsync();
                }
                _notifyService.Success("Time Added!!!");

            }
            else
            {
                _notifyService.Error("Time already exist!!!");
                return RedirectToAction("Index", new { ShowDateID = Id });

            }

        }
        return RedirectToAction("Index", new { ShowDateID = Id });
    }
    [HttpPost]
    public async Task<IActionResult> Delete(long ShowTimeID)
    {
        var showTime = _context.ShowTimes.FirstOrDefault(x => x.ID == ShowTimeID);
        _context.ShowTimes.Remove(showTime);
        _context.ShowSeats.RemoveRange(_context.ShowSeats.Where(x=>x.ShowTimeID==showTime.ID).ToList());
        await _context.SaveChangesAsync();
        _notifyService.Success("Time deleted!!!");
        return RedirectToAction("Index", new { ShowDateID = showTime.ShowDateID });
    }

}
