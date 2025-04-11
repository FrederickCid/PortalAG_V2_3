using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Models.HubSpotModels
{
    public class OwnerModel
    {

        public List<Result> results { get; set; }

        public class Result
        {
            public string id { get; set; }
            public string email { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public int userId { get; set; }
            public int userIdIncludingInactive { get; set; }
            public DateTime createdAt { get; set; }
            public DateTime updatedAt { get; set; }
            public bool archived { get; set; }
            public Team[] teams { get; set; }
        }
        public class Team
        {
            public string id { get; set; }
            public string name { get; set; }
            public bool primary { get; set; }
        }

    }
}
