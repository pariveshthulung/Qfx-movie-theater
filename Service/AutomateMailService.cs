
using QFX.data;
using QFX.Manager.Interface;

namespace QFX.Service;

public class AutomateMailService : BackgroundService
{
    private readonly ApplicationDbContext _context;
    private readonly IMailSender _emailSender;

    private readonly PeriodicTimer _timer = new(TimeSpan.FromMilliseconds(2000));
    public AutomateMailService(ApplicationDbContext context, IMailSender mailSender)
    {
        _context = context;
        _emailSender = mailSender;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await _timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
        {
            // _emailSender.SendRegistrationMail("pariveshthulung@gmail.com", "This is timer");
            Console.WriteLine("hello");
        }
    }
}
