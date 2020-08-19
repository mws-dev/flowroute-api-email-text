using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService.Util;
using FlowrouteApi.DataModels;
using FlowrouteApi.Models;
using FlowrouteApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace FlowrouteApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BounceController : ControllerBase
    {
        private FlowrouteSettings Settings { get; }
        private readonly DataContext _context;

        public BounceController(IConfiguration Configuration, DataContext context)
        {
            _context = context;
            Settings = Configuration.GetSection("Flowroute").Get<FlowrouteSettings>();
        }

        [HttpPost]
        public void Post()
        {
            string body = "";
            try
            {
                using (StreamReader sr = new StreamReader(Request.Body))
                   body = sr.ReadToEnd();

                Message msg = Message.ParseMessage(body);
                if (String.IsNullOrWhiteSpace(msg?.Signature))
                {
                    dynamic m = JsonConvert.DeserializeObject(body);
                    if (m.notificationType != null && m.notificationType == "AmazonSnsSubscriptionSucceeded")
                        return;
                }
                if (msg.IsMessageSignatureValid())
                {
                    if (msg.IsSubscriptionType)
                    {
                        HttpClient client = new HttpClient();
                        _ = client.GetAsync(msg.SubscribeURL).Result;
                        return;
                    }
                    if (msg.IsNotificationType)
                    {
                        dynamic m = JsonConvert.DeserializeObject(msg.MessageText);
                        dynamic mail = m.mail;
                        if (m.notificationType == "Bounce")
                        {
                            dynamic b = m.bounce;
                            //if (b.bounceSubType != "General" && b.bounceSubType != "Suppressed" && b.bounceSubType != "MailboxFull")
                            //{
                            //    EmailService.SendEmail(new[] { Settings.BounceReporting, Settings.ErrorReporting }, Settings.DefaultFrom, "SES Non-General Bounce", body, false);
                            //    goto END;
                            //}
                            foreach (dynamic recipient in b.bouncedRecipients)
                            {
                                string bounceBody = "<p>Following are the details of the email bounce. Ensure this email exists or remove it from the system that sent the email.</p>";
                                string bouncedEmails = "";
                                foreach (dynamic em in b.bouncedRecipients)
                                {
                                    if (bouncedEmails.Length > 0)
                                        bouncedEmails += "," + (string)em.emailAddress;
                                    else bouncedEmails = (string)em.emailAddress;
                                }
                                bounceBody += $"<p><strong>Email(s):</strong> {bouncedEmails}</p>";
                                bounceBody += $"<p><strong>Bounce Type:</strong> {b.bounceType}*</p>";
                                bounceBody += $"<p><strong>Bounce Sub Type:</strong> {b.bounceSubType}*</p>";
                                bounceBody += $"<p><strong>Original Subject:</strong> {(string)mail.commonHeaders.subject}</p>";
                                bounceBody += $"<p><strong>Date Sent:</strong> {(string)mail.commonHeaders.date}</p>";
                                bounceBody += "<p>* <strong>Transient</strong> means temporary issue. This email address should be verified. This could indicate a full inbox or vacation mode.<br />";
                                bounceBody += "* <strong>Permanent</strong> means this email address does not exist. Remove this address from the system that sent it. Contact support@example.com if you need assistance.</p>";
                                bounceBody += "<h2 style='margin-top:30px;'>Headers</h2><ul>";

                                foreach (dynamic h in mail.headers)
                                    bounceBody += $"<li><strong>{h.name}:</strong> {h.value}</li>";

                                bounceBody += "</ul>";

                                EmailService.SendEmail(new[] { Settings.BounceReporting }, Settings.DefaultFrom, "Bounce From example.com", bounceBody);
                                EmailService.SendEmail(new[] { Settings.ErrorReporting }, Settings.DefaultFrom, "SES example.com Bounce", bounceBody);
                                return;
                            }
                        }
                        else if (m.notificationType == "Complaint")
                        {
                            try
                            {
                                dynamic c = m.complaint;

                                string complaintBody = "<p>Following are the details of the email complaint. <span style='font-weight:bold;color:#f00;'>Automatic action is not available at this time.</span> Please take the appropriate action (eg - disable email, reach out to customer, etc).</p>";
                                string complainedEmails = "";
                                foreach (dynamic em in c.complainedRecipients)
                                {
                                    if (complainedEmails.Length > 0)
                                        complainedEmails += "," + (string)em.emailAddress;
                                    else complainedEmails = (string)em.emailAddress;
                                }
                                complaintBody += $"<p><strong>Complaint Type:</strong> {c.complaintFeedbackType}</p>";
                                complaintBody += $"<p><strong>Email(s):</strong> {complainedEmails}</p>";
                                complaintBody += $"<p><strong>Original Subject:</strong> {(string)mail.commonHeaders.subject}</p>";
                                complaintBody += $"<p><strong>Date Sent:</strong> {(string)mail.commonHeaders.date}</p>";

                                complaintBody += "<h2 style='margin-top:30px;'>Headers</h2><ul>";

                                foreach (dynamic h in mail.headers)
                                    complaintBody += $"<li><strong>{h.name}:</strong> {h.value}</li>";

                                complaintBody += "</ul>";


                                EmailService.SendEmail(new[] { Settings.BounceReporting }, Settings.DefaultFrom, "Email Complaint Notification", complaintBody);
                                EmailService.SendEmail(new[] { Settings.ErrorReporting }, Settings.DefaultFrom, "Complaint Notification", complaintBody);
                            }
                            catch (Exception ex)
                            {
                                EmailService.SendEmail(new[] { Settings.ErrorReporting }, Settings.DefaultFrom, "SES Complaint Processing Error", ExcDetails.Get(ex), false);
                                EmailService.SendEmail(new[] { Settings.ErrorReporting }, Settings.DefaultFrom, "SES Complaint Body for Error", body, false);
                            }
                        }
                        else
                        {
                            EmailService.SendEmail(new[] { Settings.ErrorReporting }, Settings.DefaultFrom, "SES Non-Bounce Notification", body, false);
                        }
                    }
                    else
                    {
                        EmailService.SendEmail(new[] { Settings.ErrorReporting }, Settings.DefaultFrom , "SES Non-NotificationType", body, false);
                    }
                }
            }
            catch (Exception ex)
            {
                EmailService.SendEmail(new[] { Settings.ErrorReporting }, Settings.DefaultFrom, "SES Bounce Processing Error", ExcDetails.Get(ex), false);
                if (!String.IsNullOrWhiteSpace(body))
                    EmailService.SendEmail(new[] { Settings.ErrorReporting }, Settings.DefaultFrom, "SES Bounce Body for Error", body, false);
            }
        }
    }
}