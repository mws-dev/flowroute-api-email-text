using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService.Util;
using Flowroute;
using Flowroute.Messaging;
using FlowrouteApi.DataModels;
using FlowrouteApi.Models;
using FlowrouteApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Newtonsoft.Json;

namespace FlowrouteApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private FlowrouteSettings Settings { get; }
        private readonly DataContext _context;

        public EmailController(IConfiguration Configuration, DataContext context)
        {
            Settings = Configuration.GetSection("Flowroute").Get<FlowrouteSettings>();
            _context = context;
        }

        [HttpPost]
        public void Post()
        {
            string body = "";
            try
            {
                using (StreamReader sr = new StreamReader(Request.Body, Encoding.UTF8, true, 1024, true))
                    body = sr.ReadToEnd();
                
                dynamic mb = JsonConvert.DeserializeObject(body);
                if (mb.notificationType != null && mb.notificationType == "AmazonSnsSubscriptionSucceeded")
                    return;
                Message msg = Message.ParseMessage(body);
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
                        //dynamic m = JsonConvert.DeserializeObject(msg.MessageText);
                        //dynamic mail = m.mail;
                        dynamic m = JsonConvert.DeserializeObject(msg.MessageText);
                        if (m.notificationType == "Delivery")
                            return;
                        byte[] content = Convert.FromBase64String((string)m.content);
                        var message = MimeMessage.Load(new MemoryStream(content));
                        string smsContent = message.TextBody;
                        string fromAddress = ((MailboxAddress)message.From[0]).Address;

                        var fromPhone = _context.OutgoingRoutes.Where(r => r.Email == fromAddress).Select(r => r.Phone).FirstOrDefault();
                        if (String.IsNullOrWhiteSpace(fromPhone))
                            return;

                        string toPhone = ((MailboxAddress)message.To[0]).Address;
                        toPhone = toPhone.Substring(0, toPhone.IndexOf("@"));

                        // TODO: Send error message back if toPhone is incorrect format or fromAddress not authorized in lookup table

                        FlowrouteClient client = new FlowrouteClient(Settings.FlowrouteAccessKey, Settings.FlowrouteSecretKey);
                        var result = client.Messaging.SendMessageAsync(Digitize(toPhone), fromPhone, smsContent.Trim()).Result;

                        if (!result.Success)
                            EmailService.SendEmail(new[] { Settings.ErrorReporting }, Settings.DefaultFrom, "Flowroute API Response", JsonConvert.SerializeObject(result), false);

                        //EmailService.SendEmail(new[] { Settings.ErrorReporting }, Settings.DefaultFrom, "Email Received", msgBody, false);
                        //EmailService.SendEmail(new[] { Settings.ErrorReporting }, Settings.DefaultFrom, "SMS Content", smsContent.Trim() + " - " + Digitize(toPhone) + " - " + fromAddress + " - " + fromPhone, false);
                        //EmailService.SendEmail(new[] { Settings.ErrorReporting }, Settings.DefaultFrom, "SES Non-Bounce Notification", msgBody, false);
                    }
                    else
                    {
                        EmailService.SendEmail(new[] { Settings.ErrorReporting }, Settings.DefaultFrom , "SES Non-NotificationType", body, false);
                    }
                }
            }
            catch (Exception ex)
            {
                EmailService.SendEmail(new[] { Settings.ErrorReporting }, Settings.DefaultFrom, "SES SMS Receipt Error", ExcDetails.Get(ex), false);
                if (!String.IsNullOrWhiteSpace(body))
                    EmailService.SendEmail(new[] { Settings.ErrorReporting }, Settings.DefaultFrom, "SES SMS Receipt Body for Error",body, false);
            }
        }

        private string Digitize(string number)
        {
            string num = Regex.Replace(number, @"([^0-9]+)", "");
            if (num.StartsWith("1") && num.Length > 10)
                return num;

            return "1" + num;
        }
    }
}