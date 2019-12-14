using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using SendGrid;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Configuration;

namespace asp_net_core_belarus.Services
{
    public class EmailSender : IEmailSender
    {
        private IConfiguration configuration;
        private AuthMessageSenderOptions options;

        public EmailSender(IConfiguration config)
        {
            configuration = config;
            options = new AuthMessageSenderOptions(config["SendGridUser"], config["SendGridKey"]);
        }


        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(options.SendGridKey, subject, message, email);
        }

        public Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("Northwind@mail.com", options.SendGridUser),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
    }
}
