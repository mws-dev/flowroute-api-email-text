using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlowrouteApi.Models
{
    public class FlowrouteSettings
    {
        public string AmazonSESUsername { get; set; }
        public string AmazonSESPassword { get; set; }
        public string BounceReporting { get; set; }
        public string ConnectionString { get; set; }
        public string DefaultFrom { get; set; }
        public string ErrorReporting { get; set; }
        public string FlowrouteAccessKey { get; set; }
        public string FlowrouteSecretKey { get; set; }
    }
}
