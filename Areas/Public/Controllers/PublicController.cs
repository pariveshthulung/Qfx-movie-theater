using System.Net.Http;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QFX.data;
using QFX.Migrations;
using QFX.Provider.Interface;
using QFX.ViewModels.PublicVm;
using QFX.ViewModels.TicketVm;

namespace QFX.Areas.Public.Controllers;
[Area("Public")]
[AllowAnonymous]
public class PublicController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ICurrentUserProvider _currentUser;
    private readonly ICurrentLocationProvider _currentLocation;

    public PublicController(ApplicationDbContext context, ICurrentUserProvider currentUserProvider, ICurrentLocationProvider currentLocation)
    {
        _context = context;
        _currentUser = currentUserProvider;
        _currentLocation = currentLocation;
    }
    [HttpGet]
    public async Task<IActionResult> IndexAsync(long? ID)
    {
        var vm = new IndexVm();
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
        var movieID = _context.Shows.Where(x=>x.ID==ShowID).Select(x=>x.MovieID).FirstOrDefault();
        var showIDs = _context.Shows.Include(x=>x.Audi).Where(x=>x.MovieID==movieID && x.Audi.LocationID==currentLocationId).Select(x=>x.ID).ToList();
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

    public async Task<IActionResult> MyTicket()
    {
        var currentUserID = _currentUser.GetCurrentUserId();
        var vm = new TicketIndexVm();
        
        vm.Reservations = await _context.Reservations.Where(x => x.UserID == currentUserID).ToListAsync();

        return View(vm);
    }



    [HttpGet]
    public async Task<IActionResult> GetTime(string date, long movieID)
    {
        // var currentLocationId = _currentLocation.GetCurrentLocationIDAsync();

        // var audiID = _context.Audis.Where(x => x.LocationID == currentLocationId).Select(x => x.ID).ToList();
        DateTime dateTime = DateTime.Parse(date);
        var dates = _context.ShowDates.ToList();
        var locationID = _currentLocation.GetCurrentLocationIDAsync();
        // var showDatesList = _context.ShowDates.Include(x=>x.Show).Where(x=>x.Date==dateTime && audiID.Contains(x.Show.AudiID)).ToList();
        var showDateIDs = _context.ShowDates.Include(x => x.Show).ThenInclude(y=>y.Audi).Where(x => x.Date == dateTime && (x.Show.MovieID == movieID) && (x.Show.Audi.LocationID==locationID)).Select(x => x.ID).ToList();
        var showTime = await _context.ShowTimes.Where(x => showDateIDs.Contains(x.ShowDateID)).ToListAsync();
        return Json(new { data = showTime });
    }

    [HttpGet]
    public IActionResult GetSeat(long showtimeID)
    {
        var ShowSeats = _context.ShowSeats.Where(x => x.ShowTimeID == showtimeID).Include(x => x.Seat).ThenInclude(y => y.Audi).ToList();
        return Json(new { data = ShowSeats });
    }

}