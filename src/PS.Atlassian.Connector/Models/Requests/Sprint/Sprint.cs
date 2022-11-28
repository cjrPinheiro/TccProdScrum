using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.Atlassian.Connector.Models.Requests.Sprint
{
    public class Sprint
    {
        public int id { get; set; }
        public string self { get; set; }
        public string state { get; set; }
        public string name { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public DateTime completeDate { get; set; }
        public int originBoardId { get; set; }
        public string goal { get; set; }
    }
}
