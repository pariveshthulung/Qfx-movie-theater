using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QFX.Constants;
using QFX.data;
using QFX.Migrations;
using QFX.Models;
using QFX.Provider.Interface;
using QFX.ViewModels.ShowVm;
using QFX.ViewModels.DateVm;

namespace QFX.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin,Employee")]
public class ShowController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly INotyfService _notifyService;
    private readonly ICurrentUserProvider _currentUser;

    public ShowController(ApplicationDbContext context, INotyfService notyfService, ICurrentUserProvider currentUser)
    {
        _context = context;
        _notifyService = notyfService;
        _currentUser = currentUser;
    }
    public IActionResult Index()
    {
        var vm = new IndexVm();
        var currentUserID = _currentUser.GetCurrentUserId();
        var locationID = _context.Users.Where(x => x.ID == currentUserID).Select(x => x.LocationID).FirstOrDefault();
        if (currentUserID != null)
        {
            vm.Shows = _context.Shows.Include(x => x.Audi).ThenInclude(y => y.Location).Where(x => x.Audi.LocationID == locationID).Include(x => x.Movie).ToList();
        }
        else
        {
            vm.Shows = _context.Shows.Include(x => x.Audi).ThenInclude(y => y.Location).Include(x => x.Movie).ToList();
        }

        return View(vm);
    }
    [HttpGet]
    public IActionResult Upsert(long? ShowID)
    {
        var vm = new UpsertVm();
        if (ShowID != null)
        {

            var show = _context.Shows.Where(x => x.ID == ShowID).FirstOrDefault();
            vm.AudiID = show.AudiID;
            vm.ShowStatus = show.ShowStatus;
            vm.MovieID = show.MovieID;
        }
        long? currentUserID = _currentUser.GetCurrentUserId();
        var currentUser = _context.Users.Where(x => x.ID == currentUserID).FirstOrDefault();
        long? userLocationId = currentUser.LocationID;

        vm.Audis = _context.Audis.Include(x => x.Location).Where(x => x.LocationID == userLocationId).ToList();
        vm.Movies = _context.Movies.ToList();

        return View(vm);
    }
    [HttpPost]
    public async Task<IActionResult> Upsert(long? ShowID, UpsertVm vm)
    {
        if (ShowID != null)
        {
            var show = _context.Shows.FirstOrDefault(x => x.ID == ShowID);
            show.ShowStatus = vm.ShowStatus;
            show.AudiID = vm.AudiID;

            show.MovieID = vm.MovieID;
            // if (show.AudiID == vm.AudiID)
            // {
            //     show.AudiID = vm.AudiID;
            // }
            // else
            // {
            show.AudiID = vm.AudiID;
            // var seatId = _context.Seats.Where(x => x.AudiID == vm.AudiID).Select(X => X.ID).ToList();
            // _context.RemoveRange(_context.ShowSeats.Where(x => seatId.Contains(x.SeatID)).ToList());
            // await _context.SaveChangesAsync();
            // var seatId = _context.Seats.Where(x => x.AudiID == vm.AudiID).Select(X => X.ID).ToList();

            // }
            await _context.SaveChangesAsync();
            _notifyService.Success("Show updated");
            return RedirectToAction("Index");
        }
        else
        {
            var show = new Show();
            show.ShowStatus = vm.ShowStatus;

            show.MovieID = vm.MovieID;
            show.AudiID = vm.AudiID;
            _context.Shows.Add(show);
            await _context.SaveChangesAsync();

            // var seatId = _context.Seats.Where(x => x.AudiID == vm.AudiID).Select(X => X.ID).ToList();
            // foreach (var id in seatId)
            // {
            //     var showSeat = new ShowSeat();
            //     showSeat.ShowID = show.ID;
            //     showSeat.SeatID = id;
            //     showSeat.ShowSeatStatus = SeatStatusConstants.Active;
            //     _context.ShowSeats.Add(showSeat);
            //     await _context.SaveChangesAsync();
            // }
            _notifyService.Success("Show Added!!");
            return RedirectToAction("Index");
        }
    }


    public async Task<IActionResult> Delete(long ID)
    {
        try
        {
            _context.Shows.Remove(_context.Shows.Where(x => x.ID == ID).FirstOrDefault());
            // _context.ShowSeats.RemoveRange(_context.ShowSeats.Where(x => x.ShowID == ID).ToList());
            await _context.SaveChangesAsync();
            _notifyService.Success("Show deleted!!");

            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            return Content(e.Message);
        }

    }

}
