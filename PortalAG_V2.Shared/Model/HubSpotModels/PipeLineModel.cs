using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Models.HubSpotModels
{  
    public class ResponseSearchModel
    {
        [JsonProperty("total")]
        public int total { get; set; }
        [JsonProperty("results")]
        public List<Result> results { get; set; }


        public class Result
        {
            [JsonProperty("id")]
            public string id { get; set; }
            public Properties properties { get; set; }
            public string createdAt { get; set; }
            public string updatedAt { get; set; }
            public bool archived { get; set; }
        }

        public class Properties
        {
            [JsonProperty("content")]
            public string content { get; set; }
            public string createdate { get; set; }
            public string hs_lastmodifieddate { get; set; }
            public string hs_object_id { get; set; }
            public string hs_pipeline_stage { get; set; }
            public string hubspot_owner_id { get; set; }
            [JsonProperty("rut")]
            public string rut { get; set; }
            public string subject { get; set; }
            public string hs_pipeline { get; set; }
            public string closed_date { get; set; }

        }

    }
}
