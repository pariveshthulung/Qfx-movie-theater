using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QFX.data;
using QFX.Models;
using QFX.ViewModels.GenreVm;

namespace QFX.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class GenreController : Controller
{
    private readonly INotyfService _notifyService;
    private readonly ApplicationDbContext _context;
    public GenreController(ApplicationDbContext context, INotyfService notyfService)
    {
        _notifyService = notyfService;
        _context = context;
    }
    public IActionResult Index()
    {
        var vm = new IndexVm();
        vm.Genres = _context.Genres.ToList();
        return View(vm);
    }
    public IActionResult add()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Add(UpsertVm vm)
    {
        try
        {
            var genre = new Genre();
            genre.Name = vm.Name;
            _context.Add(genre);
            await _context.SaveChangesAsync();
            _notifyService.Success("Genre added successfully");
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _notifyService.Error(e.Message);
            return RedirectToAction("Index");
        }
    }

    public async Task<IActionResult> Update(long ID)
    {
        var genre = await _context.Genres.FirstOrDefaultAsync(x => x.ID == ID);
        var vm = new UpsertVm();
        vm.Name = genre.Name;
        return View(vm);

    }
    [HttpPost]
    public async Task<IActionResult> Update(long ID, UpsertVm vm)
    {
        try
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(x => x.ID == ID);
            genre.Name = vm.Name;
            await _context.SaveChangesAsync();
            _notifyService.Success("Genre updated");
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _notifyService.Error("Fail to update genre");
            return RedirectToAction("Index");
        }
    }
    public async Task<IActionResult> Delete(long ID)
    {
        try
        {

            _context.Genres.Remove(_context.Genres.FirstOrDefault(x => x.ID == ID));
            await _context.SaveChangesAsync();
            _notifyService.Success("Genre deleted");
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _notifyService.Error("Fail to delete genre");
            return RedirectToAction("Index");
        }
    }

}
