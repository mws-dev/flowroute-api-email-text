using FlowrouteApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FlowrouteApi.Services
{
    public static class EmailService
    {
        internal static ILogger _logger;

        public static void SendEmail(string[] to, string from, string subject, string body, bool bodyHtml = true, List<Attachment> attachments = null)
        {
            using (MailMessage m = new MailMessage())
            {
                foreach (string em in to)
                    m.To.Add(em);

                m.From = new MailAddress(from);
                m.Subject = subject;
                m.Body = body;
                m.IsBodyHtml = bodyHtml;
                if (attachments != null)
                {
                    foreach (var a in attachments)
                        m.Attachments.Add(a);
                }

                using (SmtpClient c = new SmtpClient())
                {
                    c.Host = "email-smtp.us-west-2.amazonaws.com";
                    c.Port = 587;
                    c.EnableSsl = true;
                    c.UseDefaultCredentials = false;
                    c.Credentials = new NetworkCredential("", "");

                    try
                    {
                        c.Send(m);
                    }
                    catch (Exception ex) 
                    {
                        _logger.LogError(ex, ex.Message);
                    }
                }
            }
        }
    }
}
