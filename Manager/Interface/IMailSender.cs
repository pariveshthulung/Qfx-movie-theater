using QFX.ViewModels.TicketVm;

namespace QFX.Manager.Interface;

public interface IMailSender
{
    void SendRegistrationMail(string toEmail,string subject);
    void SendTicketNotify(TicketNotifyVm vm);

    void SendOTP(string toEmail,long OTP);
    void CustomMail(string toEmail,string subject,string body);
}
