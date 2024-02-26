using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QFX.data;
using QFX.Migrations;
using QFX.Models;
using QFX.ViewModels.PublicVm;

namespace QFX.Areas.Public.Controllers;
[Area("Public")]
[AllowAnonymous]
public class PublicController : Controller
{
    private readonly ApplicationDbContext _context;
    public PublicController(ApplicationDbContext context)
    {
        _context = context;
    }
    [HttpGet]
    public async Task<IActionResult> IndexAsync(long? ID)
    {
        var vm = new IndexVm();

        vm.Shows = await _context.Shows.Include(x => x.Movie).ToListAsync();
        
        // var movieIds = _context.Shows.Select(x => x.MovieID).Distinct().ToList();
        // var data = _context.Movies.Where(x => movieIds.Contains(x.ID)).ToList();

        // vm.Shows = _context.Shows.GroupBy(x => x.MovieID).Select(x => x.Select(i => i.Movie).FirstOrDefault()).ToList();

        // vm.Shows = _context.Shows.GroupBy(x=> x.MovieID).Select(x=>)
      

        vm.Locations = await _context.Locations.ToListAsync();
        vm.LocationID = ID;
        return View(vm);
    }

    public async Task<IActionResult> DetailAsync(long ID)
    {
        var vm = new DetailVm();
         vm.Movies = await _context.Movies.Where(x=>x.ID==ID)
                    .Include(x=>x.Language)
                    .Include(x=>x.MovieGenres)
                        .ThenInclude(y=>y.Genre)
                    .FirstOrDefaultAsync();
        return View(vm);
    }

    public async Task<IActionResult> BuyTicketAsync(long ShowID)
    {
        var vm = new BuyTicketVm();
        vm.Shows = await _context.Shows.Where(x => x.ID == ShowID)
                    .Include(x => x.Movie)
                        .ThenInclude(y=>y.MovieGenres)
                            .ThenInclude(z=>z.Genre)
                    .Include(x => x.Movie)
                        .ThenInclude(y=>y.Language)
                    .Include(x=>x.Audi)
                        .ThenInclude(y=>y.Location)
                    .FirstOrDefaultAsync();
        long audiID = _context.Shows.Where(x => x.ID == ShowID)
                        .Select(x => x.AudiID)
                        .FirstOrDefault();
        var locationID = _context.Shows.Where(x=>x.ID == ShowID).Include(x=>x.Audi)
                        .ThenInclude(y=>y.Location).Select(y=>y.ID);
        var seatsID = _context.Seats.Where(x => x.AudiID == audiID).ToList();
        vm.ShowSeats = _context.ShowSeats.Where(x=>x.ShowID==ShowID).Include(x=>x.Seat).OrderBy(x=>x.Seat.SeatName).ToList();
        // vm.ShowSeats = await _context.ShowSeats.Where(x=>x.SeatID==seatsID).ToList();

        return View(vm);
    }
}