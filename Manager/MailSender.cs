using System.Net;
using System.Net.Mail;
using System.Text;
using QFX.Manager.Interface;
using QFX.ViewModels.TicketVm;

namespace QFX.Manager;

public class MailSender : IMailSender
{
    public void CustomMail(string toEmail, string subject, string body)
    {
        // Set up SMTP client
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("qfxclone@gmail.com", "xcjz qobc dbna zgun");

            // Create email message
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("qfxclone@gmail.com");
            mailMessage.To.Add(toEmail);
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            StringBuilder mailBody = new StringBuilder();
            mailBody.AppendFormat(body);
            mailMessage.Body = mailBody.ToString();

            // Send email
            client.Send(mailMessage);
    }

    public void SendOTP(string toEmail, long OTP)
    {
        // Set up SMTP client
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("qfxclone@gmail.com", "xcjz qobc dbna zgun");

            // Create email message
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("qfxclone@gmail.com");
            mailMessage.To.Add(toEmail);
            mailMessage.Subject = "Verification Code";
            mailMessage.IsBodyHtml = true;
            StringBuilder mailBody = new StringBuilder();
            mailBody.AppendFormat("<h1>"+OTP+" is your verification code</h1>");
            mailBody.AppendFormat("<br />");
            mailBody.AppendFormat("<p>Do not share with anybody.Valid for 3 minutes only.</p>");
            mailMessage.Body = mailBody.ToString();

            // Send email
            client.Send(mailMessage);
    }

    public void SendRegistrationMail(string toEmail, string subject)
    {
        // Set up SMTP client
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("qfxclone@gmail.com", "xcjz qobc dbna zgun");

            // Create email message
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("qfxclone@gmail.com");
            mailMessage.To.Add(toEmail);
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            StringBuilder mailBody = new StringBuilder();
            mailBody.AppendFormat("<h1>User Registered</h1>");
            mailBody.AppendFormat("<br />");
            mailBody.AppendFormat("<p>Thank you For Registering account</p>");
            mailMessage.Body = mailBody.ToString();

            // Send email
            client.Send(mailMessage);

    }

    public void SendTicketNotify(TicketNotifyVm vm)
    {
         SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("qfxclone@gmail.com", "xcjz qobc dbna zgun");
        foreach(var clientUser in vm.TicketNotifies)
        {
            // Create email message
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("qfxclone@gmail.com");
            mailMessage.To.Add(clientUser.User.Email);
            mailMessage.Subject = clientUser.Movie.Title+ "Ticket are out now!!!";
            mailMessage.IsBodyHtml = true;
            StringBuilder mailBody = new StringBuilder();
            mailBody.AppendFormat("<h1>Book your seat!!!</h1>");
            mailBody.AppendFormat("<br />");
            mailBody.AppendFormat("<p>Ticket are available.Book your seat and enjoy movie.</p>");
            mailMessage.Body = mailBody.ToString();

            // Send email
            client.Send(mailMessage);
        }
    }
}
