using Demo.DAL.Models;
using Demo.PL.Settings;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net;
using MailKit.Net.Smtp;

namespace Demo.PL.Helpers
{
    public  class EmailSettings : IEmailSettings
    {
        private MailSettings _options;

                /*
        public static void SendEmail(Email email)
        {
        var Client =new SmtpClient("smtp.gmail.com", 587);
        Client.EnableSsl = true;
        Client.Credentials = new NetworkCredential("mohammedhanymaher990@gmail.com", "plpslbhcbpjqtqlj");
        Client.Send("mohammedhanymaher990@gmail.com", email.To, email.Subject, email.Body);
        }
        */
        public EmailSettings(IOptions<MailSettings> options)
        {
            _options = options.Value;
        }

        public void SendEmail(Email email)
        {
            var mail = new MimeMessage
            {
                // To determine email that I'll send from, <from appsettings>
                Sender = MailboxAddress.Parse(_options.Email),
                Subject = email.Subject,//subject from email that passed to function
            };

            // To determine email that I'll send to
            mail.To.Add(MailboxAddress.Parse(email.To));


            // Build The Email Body 
            var builder = new BodyBuilder();
            builder.TextBody = email.Body;
            mail.Body = builder.ToMessageBody();
            // To put the DisplayName not sender's email
            mail.From.Add(new MailboxAddress(_options.DisplayName, _options.Email));

            // To Connect to Mail Provider -> smtp
            using var smtp = new SmtpClient();
            smtp.Connect(_options.Host, _options.Port,SecureSocketOptions.StartTls);
            smtp.Authenticate(_options.Email, _options.Password);
            smtp.Send(mail);
            smtp.Disconnect(true);

        }
    }
}
