using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QFX.Constants;
using QFX.data;
using QFX.Models;
using QFX.Provider.Interface;
using QFX.ViewModels.ShowVm;

namespace QFX.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ShowController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly INotyfService _notifyService;
    private readonly ICurrentUserProvider _currentUser;

    public ShowController(ApplicationDbContext context, INotyfService notyfService,ICurrentUserProvider currentUser)
    {
        _context = context;
        _notifyService = notyfService;
        _currentUser = currentUser;
    }
    public IActionResult Index()
    {
        var vm = new IndexVm();
        vm.Shows = _context.Shows.Include(x=>x.Audi).ThenInclude(y=>y.Location).Include(x=>x.Movie).ToList();
        return View(vm);
    }
    public IActionResult Upsert()
    {
        var vm = new UpsertVm();
        long? currentUserID = _currentUser.GetCurrentUserId();
        var currentUser = _context.Users.Where(x=>x.ID==currentUserID).FirstOrDefault();
        long? userLocationId = currentUser.LocationID;

        vm.Audis = _context.Audis.Include(x => x.Location).Where(x=>x.LocationID==userLocationId).ToList();
        vm.Movies = _context.Movies.ToList();

        return View(vm);
    }
    [HttpPost]
    public async Task<IActionResult> Upsert(long? ID, UpsertVm vm)
    {
        if (ID != null)
        {
            var show = _context.Shows.FirstOrDefault(x => x.ID == ID);
            show.ShowStatus = vm.ShowStatus;
            show.Time = vm.Time;
            show.Date = vm.Date;
            show.MovieID = vm.MovieID;
            show.AudiID = vm.AudiID;
            await _context.SaveChangesAsync();
            _notifyService.Success("Show updated");
            return RedirectToAction("Index");
        }
        else
        {
            var show = new Show();
            show.ShowStatus = vm.ShowStatus;
            show.Time = vm.Time;
            show.Date = vm.Date;
            show.MovieID = vm.MovieID;
            show.AudiID = vm.AudiID;
            _context.Shows.Add(show);
            await _context.SaveChangesAsync();
            
            var seatId = _context.Seats.Where(x => x.AudiID == vm.AudiID).Select(X => X.ID).ToList();
            foreach (var id in seatId)
            {
                var showSeat = new ShowSeat();
                showSeat.ShowID = show.ID;
                showSeat.SeatID = id;
                showSeat.ShowSeatStatus = SeatStatusConstants.Active;
                _context.ShowSeats.Add(showSeat);
                await _context.SaveChangesAsync();
            }
            _notifyService.Success("Show Added!!");
            return RedirectToAction("Index");
        }
    }

}
