using System.Net.Http;
using System.Transactions;
using AspNetCoreHero.ToastNotification.Abstractions;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QFX.data;
using QFX.Migrations;
using QFX.Models;
using QFX.Provider.Interface;
using QFX.ViewModels.PublicVm;
using QFX.ViewModels.TicketVm;
using QFX.ViewModels.UserVm;

namespace QFX.Areas.Public.Controllers;
[Area("Public")]
public class PublicController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ICurrentUserProvider _currentUser;
    private readonly ICurrentLocationProvider _currentLocation;
    private readonly INotyfService _notifyService;

    public PublicController(ApplicationDbContext context, ICurrentUserProvider currentUserProvider, ICurrentLocationProvider currentLocation, INotyfService notyfService)
    {
        _context = context;
        _currentUser = currentUserProvider;
        _currentLocation = currentLocation;
        _notifyService = notyfService;

    }
    [HttpGet,AllowAnonymous]
    public async Task<IActionResult> IndexAsync(long? ID)
    {
        var vm = new QFX.ViewModels.PublicVm.IndexVm();
        var currentUserId = _currentUser.GetCurrentUserId();

        // get LocationID from session
        // if (HttpContext.Session.GetString("sessionKeyLocationId") != null)
        // {
        //     var sessionLocationID = HttpContext.Session.GetString("sessionKeyLocationId").ToString();
        //     vm.LocationID = Convert.ToInt64(sessionLocationID);

        // }
        // else if (currentUserId != null)
        // {
        //     vm.LocationID = await _context.UserLocationPreferences.Where(x => x.UserId == currentUserId).Select(x => x.LocationId)
        //     .FirstOrDefaultAsync();
        // }
        vm.LocationID = _currentLocation.GetCurrentLocationIDAsync();

        // distinct movie(done by sir)
        // var movieIds = _context.Shows.Select(x => x.MovieID).Distinct().ToList();
        // var data = _context.Movies.Where(x => movieIds.Contains(x.ID)).ToList();
        // // same as upperone
        // var result = _context.Shows.GroupBy(x => x.MovieID).Select(x => x.Select(i => i.Movie).FirstOrDefault()).ToList();

        // //select Audi.ID base on location
        var audiID = _context.Audis.Where(x => x.LocationID == vm.LocationID).Select(x => x.ID).ToList();
        // // select show base on Audi.ID
        // var show = _context.Shows.Where(x => audiID.Contains(x.AudiID)).ToList();
        // // filter show base on Movie.ID
        // var filterShow = show.Select(x => x.MovieID).Distinct().ToList();
        // //select movie base in filtered show
        // vm.Movies = await _context.Movies.Where(x => filterShow.Contains(x.ID)).ToListAsync();

        if (vm.LocationID != null)
        {
            if (vm.LocationID != 0)
            {

                vm.Shows = await _context.Shows.Where(x => audiID.Contains(x.AudiID)).Include(x => x.Movie).ToListAsync();
            }
            else
            {
                vm.Shows = await _context.Shows.Include(x => x.Movie).ToListAsync();

            }
        }
        else
        {
            vm.Shows = await _context.Shows.Include(x => x.Movie).ToListAsync();
        }

        return View(vm);
    }
    [AllowAnonymous]
    public async Task<IActionResult> DetailAsync(long ID)
    {
        var vm = new DetailVm();
        vm.Movies = await _context.Movies.Where(x => x.ID == ID)
                   .Include(x => x.Language)
                   .Include(x => x.MovieGenres)
                       .ThenInclude(y => y.Genre)
                   .FirstOrDefaultAsync();
        return View(vm);
    }
    [HttpGet,AllowAnonymous]
    public async Task<IActionResult> BuyTicketAsync(long ShowID, long MovieID)
    {
        var vm = new BuyTicketVm();
        var currentLocationId = _currentLocation.GetCurrentLocationIDAsync();

        // var audiID = _context.Audis.Where(x => x.LocationID == currentLocationId).Select(x => x.ID).ToList();
        vm.Show = await _context.Shows.Where(x => x.ID == ShowID).Include(x => x.Movie)
                        .ThenInclude(y => y.MovieGenres)
                            .ThenInclude(z => z.Genre).Include(x => x.Movie).ThenInclude(y => y.Language)
                    .Include(x => x.Audi)
                        .ThenInclude(y => y.Location).FirstOrDefaultAsync();
        var movieID = _context.Shows.Where(x => x.ID == ShowID).Select(x => x.MovieID).FirstOrDefault();
        var showIDs = _context.Shows.Include(x => x.Audi).Where(x => x.MovieID == movieID && x.Audi.LocationID == currentLocationId).Select(x => x.ID).ToList();
        // vm.Shows = await _context.Shows.Where(x => audiID.Contains(x.AudiID)).Where(x=>x.MovieID==MovieID).Include(x => x.Movie)
        //                 .ThenInclude(y => y.MovieGenres)
        //                     .ThenInclude(z => z.Genre).Include(x => x.Movie).ThenInclude(y => y.Language)
        //             .Include(x => x.Audi)
        //                 .ThenInclude(y => y.Location).ToListAsync();
        // vm.ShowTime = await _context.ShowTimes.Include(x => x.ShowDate).ThenInclude(y => y.Show).ThenInclude(x => x.Movie).ThenInclude(x => x.MovieGenres).ThenInclude(x => x.Genre).Where(x => audiID.Contains(x.ShowDate.Show.AudiID)).Where(x => x.ShowDate.Show.MovieID == MovieID)
        //     .Include(x => x.ShowDate).ThenInclude(x => x.Show).ThenInclude(x => x.Movie).ThenInclude(x => x.Language)
        //     .Include(x => x.ShowDate).ThenInclude(x => x.Show).ThenInclude(x => x.Audi).ThenInclude(x => x.Location).ToListAsync();
        vm.ShowDates = _context.ShowDates.Where(x => showIDs.Contains(x.ShowID)).ToList();
        // vm.Shows = await _context.Shows.Where(x => x.ID == ShowID)
        //             .Include(x => x.Movie)
        //                 .ThenInclude(y => y.MovieGenres)
        //                     .ThenInclude(z => z.Genre)
        //             .Include(x => x.Movie)
        //                 .ThenInclude(y => y.Language)
        //             .Include(x => x.Audi)
        //                 .ThenInclude(y => y.Location)
        //             .FirstOrDefaultAsync();
        // long audiID = _context.Shows.Where(x => x.ID == ShowID)
        //                 .Select(x => x.AudiID)
        //                 .FirstOrDefault();
        // var locationID = _context.Shows.Where(x => x.ID == ShowID).Include(x => x.Audi)
        //                 .ThenInclude(y => y.Location).Select(y => y.ID);
        // var seatsID = _context.Seats.Where(x => x.AudiID == audiID).ToList();
        // vm.ShowSeats = await _context.ShowSeats.Where(x=>x.SeatID==seatsID).ToList();



        // vm.ShowSeats = _context.ShowSeats.Where(x => x.ShowID == ShowID).Include(x => x.Seat).OrderBy(x => x.Seat.SeatName).ToList();
        // var ShowTimeIDs = await _context.ShowTimes.Include(x=>x.ShowDate).ThenInclude(y=>y.Show).Where(x=>x.ShowDate.Show.ID==ShowID).Select(x=>x.ID).ToListAsync();

        // vm.ShowSeats = _context.ShowSeats.Where(x => x.ShowTimeID == vm.ShowTime.ID).Include(x => x.Seat).OrderBy(x => x.Seat.SeatName).ToList();

        return View(vm);
    }
    [Authorize]
    public async Task<IActionResult> MyTicket()
    {
        var currentUserID = _currentUser.GetCurrentUserId();
        var vm = new TicketIndexVm();
        var reservationIds = _context.ReservationSeats.Include(x => x.Reservation).Where(x => x.Reservation.UserID == currentUserID).Select(x => x.ReservationID).ToList();
        var distinctReservationId = reservationIds.Distinct().ToList();
        vm.Reservations = await _context.Reservations
                            .Include(x => x.Show).ThenInclude(y => y.Movie)
                            .Include(x => x.ShowTime).ThenInclude(y => y.ShowDate)
                            .Where(x => distinctReservationId.Contains(x.ID))
                            .ToListAsync();

        return View(vm);
    }
    [AllowAnonymous]
    public IActionResult NowShowing()
    {
        var currentLocationId = _currentLocation.GetCurrentLocationIDAsync();
        var vm = new MovieVm();
        vm.Shows = _context.Shows.Include(x => x.Movie).Include(x => x.Audi).Where(x => x.Audi.LocationID == currentLocationId && x.ShowStatus == "Now Showing").ToList();
        return View(vm);
    }
    [AllowAnonymous]
    public IActionResult ComingSoon()
    {
        var currentLocationId = _currentLocation.GetCurrentLocationIDAsync();
        var vm = new MovieVm();
        vm.Shows = _context.Shows.Include(x => x.Movie).Include(x => x.Audi).Where(x => x.Audi.LocationID == currentLocationId && x.ShowStatus == "Coming soon").ToList();
        return View(vm);
    }
    [Authorize]
    public IActionResult UserProfile()
    {
        var currentUser = _context.Users.Where(x=>x.ID == _currentUser.GetCurrentUserId()).FirstOrDefault();
        var vm = new UpsertVm();
        vm.Email = currentUser.Email;
        vm.Name = currentUser.Name;
        vm.PhoneNo = currentUser.PhoneNo;
        vm.UserStatus = currentUser.UserStatus;
        vm.DateOfBirth = currentUser.DateOfBirth;
        return View(vm);
    }
    [HttpPost,Authorize]
    public async Task<IActionResult> Update(UpsertVm vm)
    {
        try
        {   using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var user = await _context.Users.FirstOrDefaultAsync(x=>x.ID==_currentUser.GetCurrentUserId());
            user.Name = vm.Name;
            user.PhoneNo = vm.PhoneNo;
            user.Email = vm.Email;
            user.DateOfBirth = vm.DateOfBirth;
            await _context.SaveChangesAsync();
            tx.Complete();
            _notifyService.Success("User updated!!!");
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _notifyService.Error("Operation fail" + e.Message);
            return RedirectToAction("Index");
        }
    }

    [Authorize,HttpPost]
    public async Task<IActionResult> TicketNotifyAsync(long movieID)
    {
        var ticketNotifyExist = _context.TicketNotifies.Any(x => x.MovieID == movieID && x.UserID == _currentUser.GetCurrentUserId());
        if (!ticketNotifyExist)
        {
            var TicketNotifies = new TicketNotify();
            TicketNotifies.UserID = (long)_currentUser.GetCurrentUserId();
            TicketNotifies.MovieID = movieID;
            _context.Add(TicketNotifies);
            await _context.SaveChangesAsync();
            _notifyService.Success("You will get Email!!!");
        }
        else
        {
            _notifyService.Information("You have already marked for notification!!!");
        }
        return RedirectToAction("detail", new { ID = movieID });
    }



    [HttpGet,AllowAnonymous]
    public async Task<IActionResult> GetTime(string date, long movieID)
    {
        // var currentLocationId = _currentLocation.GetCurrentLocationIDAsync();

        // var audiID = _context.Audis.Where(x => x.LocationID == currentLocationId).Select(x => x.ID).ToList();
        DateTime dateTime = DateTime.Parse(date);
        var dates = _context.ShowDates.ToList();
        var locationID = _currentLocation.GetCurrentLocationIDAsync();
        // var showDatesList = _context.ShowDates.Include(x=>x.Show).Where(x=>x.Date==dateTime && audiID.Contains(x.Show.AudiID)).ToList();
        var showDateIDs = _context.ShowDates.Include(x => x.Show).ThenInclude(y => y.Audi).Where(x => x.Date == dateTime && (x.Show.MovieID == movieID) && (x.Show.Audi.LocationID == locationID)).Select(x => x.ID).ToList();
        var showTime = await _context.ShowTimes.Where(x => showDateIDs.Contains(x.ShowDateID)).ToListAsync();
        return Json(new { data = showTime });
    }

    [HttpGet,AllowAnonymous]
    public IActionResult GetSeat(long showtimeID)
    {
        var ShowSeats = _context.ShowSeats.Where(x => x.ShowTimeID == showtimeID).Include(x => x.Seat).ThenInclude(y => y.Audi).ToList();
        return Json(new { data = ShowSeats });
    }

}

// 192.168.1.67