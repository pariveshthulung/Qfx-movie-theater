using System.Net.Http;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QFX.data;
using QFX.Migrations;
using QFX.Models;
using QFX.Provider.Interface;
using QFX.Session;
using QFX.ViewModels.PublicVm;

namespace QFX.Areas.Public.Controllers;
[Area("Public")]
[AllowAnonymous]
public class PublicController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ICurrentUserProvider _currentUser;
    private readonly ICurrentLocationProvider _currentLocation;

    public PublicController(ApplicationDbContext context, ICurrentUserProvider currentUserProvider,ICurrentLocationProvider currentLocation)
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

    public async Task<IActionResult> BuyTicketAsync(long ShowID,long MovieID)
    {
        var vm = new BuyTicketVm();
        var currentLocationId = _currentLocation.GetCurrentLocationIDAsync();

        var audiID = _context.Audis.Where(x => x.LocationID == currentLocationId).Select(x => x.ID).ToList();
        vm.Shows = await _context.Shows.Where(x => audiID.Contains(x.AudiID)).Where(x=>x.MovieID==MovieID).Include(x => x.Movie)
                        .ThenInclude(y => y.MovieGenres)
                            .ThenInclude(z => z.Genre).Include(x => x.Movie).ThenInclude(y => y.Language)
                    .Include(x => x.Audi)
                        .ThenInclude(y => y.Location).ToListAsync();

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

        return View(vm);
    }
}