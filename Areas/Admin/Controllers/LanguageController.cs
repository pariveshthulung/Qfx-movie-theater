using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QFX.data;
using QFX.Models;
using QFX.ViewModels.LanguageVm;

namespace QFX.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin,Employee")]
public class LanguageController : Controller
{
    private readonly INotyfService _notifyService;
    private readonly ApplicationDbContext _context;

    public LanguageController(ApplicationDbContext context, INotyfService notyfService)
    {
        _notifyService = notyfService;
        _context = context;
    }
    public IActionResult Index()
    {
        var vm = new IndexVm();
        vm.Languages = _context.Languages.ToList();
        return View(vm);
    }
    public async Task<IActionResult> Upsert(long? ID)
    {
        var vm = new UpsertVm();
        if (ID != null)
        {
            var language = await _context.Languages.FirstOrDefaultAsync(x => x.ID == ID);
            vm.Name = language.Name;
        }
        return View(vm);
    }
    [HttpPost]
    public async Task<IActionResult> Upsert(long? ID, UpsertVm vm)
    {
        try
        {
            if (ID != null)
            {
                var language = await _context.Languages.FirstOrDefaultAsync(x => x.ID == ID);
                language.Name = vm.Name;
            }
            else
            {
                var language = new Language();
                language.Name = vm.Name;
                _context.Languages.Add(language);
            }
            _notifyService.Success("Done successfully!!");
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _notifyService.Error("Operation fails" + e.Message);
            return View(vm);
        }
    }
    public async Task<IActionResult> Delete(long ID)
    {
        try
        {
            _context.Languages.Remove(_context.Languages.FirstOrDefault(x => x.ID == ID));
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
