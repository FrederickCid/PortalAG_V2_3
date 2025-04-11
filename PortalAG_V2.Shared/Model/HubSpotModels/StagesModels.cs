using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Models.HubSpotModels
{
    public class StagesModels
    {

        public List<Result> results { get; set; }


        public class Result
        {
            public string label { get; set; }
            public int displayOrder { get; set; }
            public string id { get; set; }
            public List<Stage> stages { get; set; }
            public DateTime createdAt { get; set; }
            public DateTime updatedAt { get; set; }
            public bool archived { get; set; }
        }

        public class Stage
        {
            public string label { get; set; }
            public int displayOrder { get; set; }
            public Metadata metadata { get; set; }
            public string id { get; set; }
            public DateTime createdAt { get; set; }
            public DateTime updatedAt { get; set; }
            public bool archived { get; set; }
            public string writePermissions { get; set; }
        }

        public class Metadata
        {
            public string ticketState { get; set; }
            public string isClosed { get; set; }
        }

    }
}
