using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.Atlassian.Connector.Models.Requests.Sprint
{
    public class SprintsResponse
    {
        public int maxResults { get; set; }
        public int startAt { get; set; }
        public bool isLast { get; set; }
        public List<Sprint> values { get; set; }
    }
}
