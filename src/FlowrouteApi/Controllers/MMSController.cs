using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
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
    public class MMSController : ControllerBase
    {
        private FlowrouteSettings Settings { get; }
        private readonly DataContext _context;
        public MMSController(IConfiguration Configuration, DataContext context)
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
                using (StreamReader sr = new StreamReader(Request.Body, Encoding.UTF8, true, 1024, true))
                    body = sr.ReadToEnd();

                FlowrouteMmsMessage msg = FlowrouteMmsMessage.FromJson(body);
                List<Attachment> attachments = new List<Attachment>();
                string toPhone = msg.Data.Attributes.To;
                string fromPhone = msg.Data.Attributes.From;
                HttpClient httpClient = new HttpClient();
                if (msg.Included != null)
                {
                    foreach (var file in msg.Included)
                    {
                        byte[] bytes = httpClient.GetByteArrayAsync(file.Attributes.Url).Result;
                        attachments.Add(new Attachment(new MemoryStream(bytes), file.Attributes.FileName, file.Attributes.MimeType));
                    }
                }
                var routes = _context.IncomingRoutes.Where(r => r.Phone == toPhone);
                if (routes.Count() > 0)
                {
                    EmailService.SendEmail(routes.Select(r => r.Email).ToArray(), $"{fromPhone}@example.com", $"MMS Message From {fromPhone}", msg.Data.Attributes.Body ?? "No body text", false, attachments);
                }
                //EmailService.SendEmail(new[] { Settings.ErrorReporting }, Settings.DefaultFrom, "Flowroute API MMS Request", JsonConvert.SerializeObject(msg), false);
            }
            catch (Exception ex)
            {
                EmailService.SendEmail(new[] { Settings.ErrorReporting }, Settings.DefaultFrom, "Flowroute API Error", ExcDetails.Get(ex), false);
                if (!String.IsNullOrWhiteSpace(body))
                    EmailService.SendEmail(new[] { Settings.ErrorReporting }, Settings.DefaultFrom, "MMS Receipt Body for Error", body, false);
            }
        }
    }
}