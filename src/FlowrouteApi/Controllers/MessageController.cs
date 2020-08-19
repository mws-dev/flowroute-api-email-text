using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flowroute;
using Flowroute.Messaging;
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
    public class MessageController : ControllerBase
    {
        private FlowrouteSettings Settings { get; }
        private readonly DataContext _context;
        public MessageController(IConfiguration Configuration, DataContext context)
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

                FlowrouteGetMessageResponse msg = JsonConvert.DeserializeObject<FlowrouteGetMessageResponse>(body);
                var routes = _context.IncomingRoutes.Where(r => r.Phone == msg.Data.Attributes.To);
                if (routes.Count() > 0)
                {
                    EmailService.SendEmail(routes.Select(r => r.Email).ToArray(), $"{msg.Data.Attributes.From}@example.com", $"SMS Message From {msg.Data.Attributes.From}", msg.Data.Attributes.Body, false);

                    //FlowrouteClient client = new FlowrouteClient(Settings.FlowrouteAccessKey, Settings.FlowrouteSecretKey);
                    //var result = await client.Messaging.SendMessageAsync(msg.Data.Attributes.From, msg.Data.Attributes.To, "Message received!");

                    //EmailService.SendEmail(new[] { Settings.ErrorReporting }, Settings.DefaultFrom, "Flowroute API Request", body, false);
                    //EmailService.SendEmail(new[] { Settings.ErrorReporting }, Settings.DefaultFrom, "Flowroute API Response", JsonConvert.SerializeObject(result), false);
                }
            }
            catch (Exception ex)
            {
                EmailService.SendEmail(new[] { Settings.ErrorReporting }, Settings.DefaultFrom, "Flowroute API Error", ExcDetails.Get(ex), false);
            }
        }
    }
}