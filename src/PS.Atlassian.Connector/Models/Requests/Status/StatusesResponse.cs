using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.Atlassian.Connector.Models.Requests.Status
{
    public class StatusesResponse
    {
        public string self { get; set; }
        public string description { get; set; }
        public string iconUrl { get; set; }
        public string name { get; set; }
        public string untranslatedName { get; set; }
        public string id { get; set; }
        public StatusCategory statusCategory { get; set; }
        public Scope scope { get; set; }

    }

    public class StatusCategory
    {
        public string self { get; set; }
        public int id { get; set; }
        public string key { get; set; }
        public string colorName { get; set; }
        public string name { get; set; }
    }

    public class Project
    {
        public string id { get; set; }
    }

    public class Scope
    {
        public string type { get; set; }
        public Project project { get; set; }
    }
}
