using System.Net;
using System.Net.Mail;

namespace blog.Services;

public class EmailService
{
    public bool Send(
        string toName,
        string toEmail,
        string subject,
        string body,
        string fromName = "ENDERSON AMORIM",
        string fromEmail = "endersonrufino@gmail.com")
    {
        var smtp = new SmtpClient(Configuration.Smtp.Host, Configuration.Smtp.Port);

        smtp.Credentials = new NetworkCredential(Configuration.Smtp.UserName, Configuration.Smtp.Password);
        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtp.EnableSsl = true;

        var mail = new MailMessage();

        mail.From = new MailAddress(fromEmail, fromName);
        mail.To.Add(new MailAddress(toEmail, toName));
        mail.Subject = subject;
        mail.Body = body;
        mail.IsBodyHtml = true;

        try
        {
            smtp.Send(mail);

            return true;
        }
        catch
        {
            return false;
        }


    }
}