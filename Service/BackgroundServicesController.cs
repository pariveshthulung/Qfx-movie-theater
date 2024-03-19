using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QFX.data;
using QFX.Generic.Interface;
using QFX.Manager.Interface;
using QFX.Models;
using QFX.ViewModels.TicketVm;
using Stripe;

namespace QFX.Service;
[Area("Admin")]
[AllowAnonymous]

public class BackgroundServicesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMailSender _emailSender;
    private readonly IDeleteShow _deleteShow;
    public BackgroundServicesController(ApplicationDbContext context,IMailSender mailSender,IDeleteShow deleteShow)
    {
        _context = context;
        _emailSender = mailSender;
        _deleteShow = deleteShow;
    }

    [Obsolete]
    public IActionResult NotifyUser()
    {
        
        var vm = new TicketNotifyVm();
        vm.TicketNotifies = _context.TicketNotifies.Include(x=>x.Movie).Include(x=>x.User).Where(x=>x.Movie.ReleaseDate==DateTime.Now).ToList();
        RecurringJob.AddOrUpdate<IMailSender>(x=>x.SendTicketNotify(vm),Cron.Daily);
        return Ok();
    }

    [Obsolete]
    public IActionResult DeleteShow()
    {
        var dueDate = _context.ShowDates.Where(x=>x.Date<DateTime.Now).Select(x=>x.ShowID).ToList();
        RecurringJob.AddOrUpdate<IDeleteShow>(x=>x.DeleteShow(dueDate),Cron.Daily); 
        return Ok();
    }
}
