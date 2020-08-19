using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FlowrouteApi.Models
{
    public partial class FlowrouteMmsMessage
    {
        [JsonProperty("included")]
        public Included[] Included { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("relationships")]
        public Relationships Relationships { get; set; }

        [JsonProperty("attributes")]
        public DataAttributes Attributes { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public partial class DataAttributes
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("direction")]
        public string Direction { get; set; }

        [JsonProperty("amount_nanodollars")]
        public long AmountNanodollars { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("message_encoding")]
        public long MessageEncoding { get; set; }

        [JsonProperty("timestamp")]
        public DateTimeOffset Timestamp { get; set; }

        [JsonProperty("delivery_receipts")]
        public object[] DeliveryReceipts { get; set; }

        [JsonProperty("amount_display")]
        public string AmountDisplay { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("is_mms")]
        public bool IsMms { get; set; }

        [JsonProperty("message_type")]
        public string MessageType { get; set; }
    }

    public partial class Relationships
    {
        [JsonProperty("media")]
        public Media Media { get; set; }
    }

    public partial class Media
    {
        [JsonProperty("data")]
        public Datum[] Data { get; set; }
    }

    public partial class Datum
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public partial class Included
    {
        [JsonProperty("attributes")]
        public IncludedAttributes Attributes { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("links")]
        public Links Links { get; set; }
    }

    public partial class IncludedAttributes
    {
        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("file_name")]
        public string FileName { get; set; }

        [JsonProperty("mime_type")]
        public string MimeType { get; set; }

        [JsonProperty("file_size")]
        public long FileSize { get; set; }
    }

    public partial class Links
    {
        [JsonProperty("self")]
        public Uri Self { get; set; }
    }

    public partial class FlowrouteMmsMessage
    {
        public static FlowrouteMmsMessage FromJson(string json)
        {
            return JsonConvert.DeserializeObject<FlowrouteMmsMessage>(json, Converter.Settings);
        }
    }

    public static class Serialize
    {
        public static string ToJson(this FlowrouteMmsMessage self)
        {
            return JsonConvert.SerializeObject(self, Converter.Settings);
        }
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
